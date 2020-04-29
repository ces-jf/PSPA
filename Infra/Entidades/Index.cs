using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infra.Entidades
{
    public class Index
    {
        public Index()
        {
            AlreadyMapped = true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string Name { get; set; }
        public bool AlreadyMapped { get; set; }
        public ICollection<ArquivoBase> ArquivoBases { get; set; }
    }
}
