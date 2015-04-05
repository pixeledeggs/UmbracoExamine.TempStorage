using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Examine;
using Examine.LuceneEngine.Config;
using Lucene.Net.Index;

namespace UmbracoExamine.TempStorage
{
    public class UmbracoTempStorageContentIndexer : UmbracoContentIndexer
    {
        private readonly UmbracoTempStorageIndexer _helper = new UmbracoTempStorageIndexer();

        public UmbracoTempStorageContentIndexer()
        {
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            var indexSet = IndexSets.Instance.Sets[IndexSetName];
            var configuredPath = indexSet.IndexPath;

            _helper.Initialize(config, configuredPath, base.GetLuceneDirectory(), IndexingAnalyzer);
        }
        
        public override Lucene.Net.Store.Directory GetLuceneDirectory()
        {
            if (_helper.LuceneDirectory == null)
            {
                throw new InvalidOperationException("The temp storage provider has not been initialized");
            }

            return _helper.LuceneDirectory;
        }

        protected override IndexWriter CreateIndexWriter()
        {
            return new IndexWriter(GetLuceneDirectory(), IndexingAnalyzer, false, IndexWriter.MaxFieldLength.UNLIMITED);
        }
    }
}
