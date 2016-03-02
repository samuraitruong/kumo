using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_kumo_eip0001model
{
    [MetadataType(typeof(DocumentLibraryMeta))]
    public partial class DocumentLibrary
    {
    }

    public class DocumentLibraryMeta {
        [Required]
        public string Name;

        [DataType(DataType.MultilineText)]
        public string Description;
    }
}
