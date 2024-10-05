namespace Cataract_Care.Models.DTOs
{
    public class CataractDiagnosisDto
    {
        public string UserName { get; set; }          
        public string PatientName { get; set; }       
        public int Age { get; set; }                  
        public string Eye { get; set; }               
        public DateTime UploadDate { get; set; }      
        public string Gender { get; set; }            
        public string Image { get; set; }             
        public string IsCataract { get; set; }        
    }

}
