using Nest;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
            var re = await elasticSearchExtend.InsertEntityAsync(user);
            Assert.IsTrue(re);
            Assert.Pass();
        }
        [Test]
        public async Task Test5()
        {
            var re = await elasticSearchExtend.FindOne("5mix73IBv-C3Ucz42PR-");
            Assert.IsTrue(re != null);
            Assert.Pass();
        }

        [Test]
        public async Task Test6()
        {
            user user = new user();
            user.Name = "z4yh";
            var re = await elasticSearchExtend.UpdateEntityAsync("5mix73IBv-C3Ucz42PR-", user);
            Assert.IsTrue(re);
            Assert.Pass();
        }

        [Test]
        public async Task Test7()
        {
            var re = await elasticSearchExtend.FindAll();
            Assert.IsTrue(re.Count > 0);
            Assert.Pass();
        }

        [Test]
        public async Task Test8()
        {
            var re = await elasticSearchExtend.DeleteEntityAsync("5mix73IBv-C3Ucz42PR-");
            Assert.IsTrue(re);
            Assert.Pass();
        }

        [Test]
        public async Task Test9()
        {
            List<user> list = new List<user>();
            list.Add(new user() { Account = "520", Age = 22, Name = "dd" });
            list.Add(new user() { Account = "521", Age = 20, Name = "mm" });
            var re = await elasticSearchExtend.InsertManyEntityAsync(list);
            Assert.IsTrue(re);
            Assert.Pass();
        }

        [Test]
        public async Task Test10()
        {
            //两种方法都行
            //.Query(q => q.Match(m => m.Field(f => f.Name).Query("其他笔记本6"))) //单字段全文关键字检索 只要Name中包含值即可，且自带分词 
            //.Query(q => q.MultiMatch(m => m.Fields(fd=>fd.Fields(f=>f.Name,f=>f.OtherInfo)).Query("1神23456789"))) //多字段全文关键字检索 Name或OtherInfo包含该值即可，且自带分词 
            //.Analyzer("") // 该分词方法可不需要，因为上面的查询自带分词
            //.Query(q => q.Bool(b=>b.Must(m=>m.Term(p=>p.Field(f=>f.Id).Value(4)))))  //条件必须符合，无分词，有一些数据类型可能查询失败
            //.Query(q => q.Range(c => c.Field(f => f.Id).LessThanOrEquals(5).GreaterThanOrEquals(3))) //范围查询
            //.Sort(t => t.Ascending(p=>p.Id)) //id升序
            //.From(0) //分页 第几条开始展示
            //.Size(3) //分页，每页显示多少条

            //多个条件一起搜索，例子如下
            //var matchQuery = new List<Func<QueryContainerDescriptor<Computer>, QueryContainer>>
            //{
            //    must => must.Bool(b => b.Must(m => m.Term(p => p.Field(f => f.Id).Value(5)),
            //                                    m => m.Term(p => p.Field(f => f.Name).Value("神州笔记本1"))
            //                                 )
            //                     ),
            //    range => range.Range(c => c.Field(p => p.Id).LessThanOrEquals(5).GreaterThanOrEquals(3))
            //};
            //var tr = es.Search<Computer>(x=>x.Index("realyuseit").Query(q=>q.Bool(b=>b.Must(matchQuery))))


            //1. 
            //SearchDescriptor<user> searchDescriptor = new SearchDescriptor<user>();
            //searchDescriptor.From(0);
            //searchDescriptor.Size(10);
            //searchDescriptor.Query(q => q.Match(m => m.Field(f => f.Name).Query("mm")));
            //var list = await elasticSearchExtend.FindWhere(searchDescriptor);

            //2.
            var searchRequest = new SearchRequest<user>(Nest.Indices.All)
            {
                From = 0,
                Size = 10,
                Query = new MatchQuery
                {
                    Field = Infer.Field<user>(f => f.Name),
                    Query = "mm"
                }
            };
            var list = await elasticSearchExtend.FindWhere(searchRequest);
            Assert.Pass();
        }
    }
}