using NUnit.Framework;
using System.Threading.Tasks;
using Zhaoxi.Helper;
using Zhaoxi.IocDI.Model;

namespace NUnitTestProject
{
    public class Tests
    {
        private ElasticSearchHelper elasticSearchHelper = new ElasticSearchHelper("http://localhost:9200");
        private ElasticSearchExtendHelper<user> elasticSearchExtend = new ElasticSearchExtendHelper<user>();

        /// <summary>
        /// 类似于构造函数
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var isexists = elasticSearchHelper.IsExistsByElasticClientIndex("zhaoxi");
            Assert.IsTrue(isexists);
            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            var isexists = elasticSearchHelper.CreateElasticClientIndex("zhaoxi1");
            Assert.IsTrue(isexists);
            Assert.Pass();
        }

        [Test]
        public void Test3()
        {
            var isexists = elasticSearchHelper.DeleteElasticClientIndex("zhaoxi1");
            Assert.IsTrue(isexists);
            Assert.Pass();
        }

        [Test]
        public async Task Test4()
        {
            user user = new user();
            user.Account = "111";
            user.Age = 19;
            user.Name = "z2";
            var re = await elasticSearchExtend.InsertEntityAsync(user) > 0 ? true : false;
            Assert.IsTrue(re);
            Assert.Pass();
        }

    }
}