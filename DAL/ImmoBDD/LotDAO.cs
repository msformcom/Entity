using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImmoBDD
{
    // Formaliser la structure des infos dans la BDD
    //[Table("Tbl_Lots")] // ancienne méthode pour spécifier le nom de la table
    public  class LotDAO 
    {
       
        public Guid Id { get; set; } = Guid.NewGuid();

        //[Column("Ref")]// ancienne méthode pour spécifier le nom de la colonne
        public string Reference { get; set; }


        public string Ligne { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }

        
        public decimal Prix { get; set; }
        public bool RezDeChaussee { get; set; }

        public DateTime LastUpdate { get; set; } = DateTime.Now;


    }
}
