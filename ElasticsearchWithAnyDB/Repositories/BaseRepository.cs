using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using ElasticsearchWithAnyDB.Models;

namespace ElasticsearchWithAnyDB.Repositories
{
    public class BaseRepository
    {
        private readonly string FILE_NAME = "products.json";
        private readonly string FILE_NAME_ARCH = "products.zip";

        public virtual IQueryable<Product> Products => new List<Product>().AsQueryable();

        private void ExtractZip()
        {
            FastZip fastZip = new FastZip();
            // Will always overwrite if target filenames already exist
            fastZip.ExtractZip(FILE_NAME_ARCH, Directory.GetCurrentDirectory(), null);
        }

        public IEnumerable<Product> LoadProductsFromFile()
        {
            ExtractZip();
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(File.ReadAllText(FILE_NAME));
        }

        public virtual IEnumerable<Product> Search(string stringSearch)
        {
            string s = stringSearch.ToUpper();
            return Products.Where(p => p.Id.ToString().ToUpper().Contains(s)
                                        || p.Name.ToUpper().Contains(s)
                                        || p.Keywords.ToUpper().Contains(s));
        }
    }
}
