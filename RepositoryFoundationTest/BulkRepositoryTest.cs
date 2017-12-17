using RepositoryFoundation.Helper.BulkRepository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryFoundation.Helper.DataTableHelper;

namespace RepositoryFoundationTest
{
    [TestClass]
    public class BulkRepositoryTest
    {
        [TestMethod]
        public void Test()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["FpsConnection"].ConnectionString;
            var apartmentList = new List<Apartment>();
            for (int index = 0; index < 10000; index++)
            {
                apartmentList.Add(new Apartment{IsDeleted = false, Number = index + 1, OnRent = ((index % 2)>0) });
            }

            var repo = new Repository(apartmentList.AsDataTable(), "Apartment", connectionString, "Id");
            repo.BulkUpsert();
        }
    }

    [Table("Apartment", Schema = "dbo")]
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.34.1.0")]
    public class Apartment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(@"Id", Order = 1, TypeName = "int")]
        [Index(@"PK_Apartment", 1, IsUnique = true, IsClustered = true)]
        [Required]
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; } // Id (Primary key)

        [Column(@"ResidentId", Order = 2, TypeName = "int")]
        [Required]
        [Display(Name = "Resident ID")]
        public int? ResidentId { get; set; } // ResidentId

        [Column(@"OnRent", Order = 3, TypeName = "bit")]
        [Required]
        [Display(Name = "On rent")]
        public bool OnRent { get; set; } // OnRent

        [Column(@"Number", Order = 4, TypeName = "int")]
        [Required]
        [Display(Name = "Number")]
        public int Number { get; set; } // Number

        [Column(@"OwnerId", Order = 5, TypeName = "int")]
        [Required]
        [Display(Name = "Owner ID")]
        public int? OwnerId { get; set; } // OwnerId

        [Column(@"IsDeleted", Order = 6, TypeName = "bit")]
        [Required]
        [Display(Name = "Is deleted")]
        public bool IsDeleted { get; set; } // IsDeleted
    }
}
