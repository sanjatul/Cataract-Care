from flask import Flask, request, jsonify
import os
import torch
import torch.nn as nn
import torch.optim as optim
from torchvision import datasets, models, transforms
from torch.utils.data import DataLoader
import matplotlib.pyplot as plt
from sklearn.metrics import confusion_matrix, classification_report, accuracy_score, roc_curve, auc
import matplotlib.pyplot as plt
import numpy as np
import time
import torch
from torchvision import transforms
from PIL import Image
from flask_cors import CORS
import pickle
import torch.nn.functional as F

# Initialize the Flask app
app = Flask(__name__)
CORS(app)
# Create an images folder if it doesn't exist
UPLOAD_FOLDER = 'images'
if not os.path.exists(UPLOAD_FOLDER):
    os.makedirs(UPLOAD_FOLDER)

# Configure the upload folder for the app
app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER

# Define the DCNN Model (using a modified AlexNet as a base)
class CataractClassifier(nn.Module):
    def __init__(self, num_classes=2):
        super(CataractClassifier, self).__init__()
        self.features = nn.Sequential(
            nn.Conv2d(1, 96, kernel_size=11, stride=4, padding=2),
            nn.ReLU(inplace=True),
            nn.MaxPool2d(kernel_size=3, stride=2),
            nn.Conv2d(96, 256, kernel_size=5, padding=2),
            nn.ReLU(inplace=True),
            nn.MaxPool2d(kernel_size=3, stride=2),
            nn.Conv2d(256, 384, kernel_size=3, padding=1),
            nn.ReLU(inplace=True),
            nn.Conv2d(384, 384, kernel_size=3, padding=1),
            nn.ReLU(inplace=True),
            nn.Conv2d(384, 256, kernel_size=3, padding=1),
            nn.ReLU(inplace=True),
            nn.MaxPool2d(kernel_size=3, stride=2)
        )
        self.classifier = nn.Sequential(
            nn.Dropout(),
            nn.Linear(256 * 6 * 6, 4096),
            nn.ReLU(inplace=True),
            nn.Dropout(),
            nn.Linear(4096, 4096),
            nn.ReLU(inplace=True),
            nn.Linear(4096, num_classes),
        )

    def forward(self, x):
        x = self.features(x)
        x = x.view(x.size(0), 256 * 6 * 6)
        x = self.classifier(x)
        return x
    
# Initialize model, loss function, and optimizer
model = CataractClassifier(num_classes=2)
criterion = nn.CrossEntropyLoss()
optimizer = optim.Adam(model.parameters(), lr=0.001)

# Function to load the model
def load_model(model_path, class_names_path):
    model = CataractClassifier(num_classes=2)
    model.load_state_dict(torch.load(model_path))
    model.eval()
    
    with open(class_names_path, 'rb') as f:
        class_names = pickle.load(f)
    
    print("Model and class names loaded successfully!")
    return model, class_names

# Get the current working directory
current_dir = os.getcwd()
models_dir = os.path.join(current_dir, 'models')

def predict_image(image_path, model, class_names):
    preprocess = transforms.Compose([
        transforms.Resize((227, 227)),
        transforms.Grayscale(num_output_channels=1),
        transforms.ToTensor(),
        transforms.Normalize([0.5], [0.5])
    ])
    
    image = Image.open(image_path)
    image = preprocess(image).unsqueeze(0)  # Add batch dimension

    with torch.no_grad():
        output = model(image)
        probabilities = F.softmax(output, dim=1)
        _, predicted = torch.max(output, 1)
        predicted_class = class_names[predicted.item()]
        confidence_score = probabilities[0, predicted.item()].item() * 100
    
    print(f'Predicted class: {predicted_class}, Confidence: {confidence_score:.2f}%')
    return predicted_class, confidence_score

# Load the model and class names
model_load_path = os.path.join(models_dir, 'Paper1_model.pkl')
class_names_load_path = os.path.join(models_dir, 'Paper1_class_names.pkl')
model, class_names = load_model(model_load_path, class_names_load_path)

# API route to handle file upload and predict the class of the uploaded image
@app.route('/upload-image', methods=['POST'])
def upload_file():
    if 'file' not in request.files:
        return jsonify({"message": "No file part in the request"}), 400
    
    file = request.files['file']
    
    if file.filename == '':
        return jsonify({"message": "No file selected"}), 400

    # Save the file to the images folder
    if file:
        file_path = os.path.join(app.config['UPLOAD_FOLDER'], file.filename)
        file.save(file_path)

        # Predict the class for the uploaded image
        predicted_class, confidence_score = predict_image(file_path, model, class_names)
        
        return jsonify({
            "predictedClass": predicted_class,
            "confidenceScore": confidence_score
        }), 200

# Run the app
if __name__ == '__main__':
    app.run(debug=True)
