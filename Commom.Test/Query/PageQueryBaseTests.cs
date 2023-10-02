using Moq;
using MyCloa.Common;
using MyCloa.Common.DataSerializer;
using MyCloa.Common.Ioc;
using MyCloa.Common.Valid;

namespace MyCloa.Common.Query
{
    [TestFixture]
    public class PageQueryBaseTests
    {
        // [Test]
        // public void PageQueryBase_Should_Return_PageResult_With_No_Summary_Data()
        // {
        //     // Arrange
        //     var query = new PageQueryBaseImpl();
        //
        //     // Act
        //     var result = query.Execute();
        //
        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.IsInstanceOf<PageResult<Data>>(result);
        // }

        [Test]
        public void PageQueryBase_Should_Return_PageResult_With_Summary_Data()
        {// Arrange
            var query = new PageQueryBaseImplWithList();
            RequestStringEntity requestStringEntity = new RequestStringEntity();
            PageRequest pageRequest = new PageRequest();
            pageRequest.PageIndex = 0;
            pageRequest.PageSize = 10;
            pageRequest.IncludeSummary = true;

            requestStringEntity.Data = System.Text.Json.JsonSerializer.Serialize(pageRequest);
            
            
            Mock<IResolve> resolveMod = new Mock<IResolve>();
            Mock<IDataSerializer> dataSerializer = new Mock<IDataSerializer>();
            Mock<IValidRequest> validRequestMod = new Mock<IValidRequest>();
            var defaultSerial = new DefaultSerializer();
            validRequestMod.Setup(p => p.Valid(It.IsAny<PageRequest>())).Returns(new ValidResult(true, ""));
            dataSerializer.Setup(p => p.Deserialize<PageRequest>(It.Is<string>(w => w == requestStringEntity.Data))).Returns(pageRequest);
            resolveMod.Setup(p => p.Resolve<IDataSerializer>()).Returns(defaultSerial);
            resolveMod.Setup(p => p.Resolve<IValidRequest>()).Returns(validRequestMod.Object);
            ((ICommand)query).Resolve = resolveMod.Object;
            // Act
            var result = query.Run(requestStringEntity);

            // Assert
            Assert.NotNull(result);
            var actResult = defaultSerial.Deserialize<PageResult<Data>>(result);
            Assert.IsInstanceOf<PageResult<Data>>(actResult);
            Assert.NotNull(actResult);
            Assert.That(actResult.TotalCount, Is.EqualTo(30));
            
            Assert.That(actResult.PageData.Count, Is.EqualTo(10));
        }
        [Test]
        public void PageQueryBase_Should_Return_PageResult_With_No_Data()
        {// Arrange
            var query = new PageQueryBaseImplWithNoList();
            RequestStringEntity requestStringEntity = new RequestStringEntity();
            PageRequest pageRequest = new PageRequest();
            pageRequest.PageIndex = 0;
            pageRequest.PageSize = 10;
            pageRequest.IncludeSummary = true;

            requestStringEntity.Data = System.Text.Json.JsonSerializer.Serialize(pageRequest);
            
            
            Mock<IResolve> resolveMod = new Mock<IResolve>();
            Mock<IDataSerializer> dataSerializer = new Mock<IDataSerializer>();
            Mock<IValidRequest> validRequestMod = new Mock<IValidRequest>();
            var defaultSerial = new DefaultSerializer();
            validRequestMod.Setup(p => p.Valid(It.IsAny<PageRequest>())).Returns(new ValidResult(true, ""));
            dataSerializer.Setup(p => p.Deserialize<PageRequest>(It.Is<string>(w => w == requestStringEntity.Data))).Returns(pageRequest);
            resolveMod.Setup(p => p.Resolve<IDataSerializer>()).Returns(defaultSerial);
            resolveMod.Setup(p => p.Resolve<IValidRequest>()).Returns(validRequestMod.Object);
            ((ICommand)query).Resolve = resolveMod.Object;
            // Act
            var result = query.Run(requestStringEntity);

            // Assert
            Assert.NotNull(result);
            var actResult = defaultSerial.Deserialize<PageResult<Data>>(result);
            Assert.IsInstanceOf<PageResult<Data>>(actResult);
            Assert.NotNull(actResult);
            Assert.That(actResult.TotalCount, Is.EqualTo(0));
            
            Assert.That(actResult.PageData.Count, Is.EqualTo(0));
        }

        [Test]
        public void PageQueryBase_With_Query_Parameters_Should_Return_PageResult_With_Summary_Data()
        {
            // Arrange
            var query = new PageQueryBaseImplWithSummary();
        
            // Act
            RequestStringEntity requestStringEntity = new RequestStringEntity();
            PageRequest pageRequest = new PageRequest();
            pageRequest.PageIndex = 0;
            pageRequest.PageSize = 10;
            pageRequest.IncludeSummary = true;

            requestStringEntity.Data = System.Text.Json.JsonSerializer.Serialize(pageRequest);
            
            
            Mock<IResolve> resolveMod = new Mock<IResolve>();
            Mock<IDataSerializer> dataSerializer = new Mock<IDataSerializer>();
            Mock<IValidRequest> validRequestMod = new Mock<IValidRequest>();
            var defaultSerial = new DefaultSerializer();
            validRequestMod.Setup(p => p.Valid(It.IsAny<PageRequest>())).Returns(new ValidResult(true, ""));
            dataSerializer.Setup(p => p.Deserialize<PageRequest>(It.Is<string>(w => w == requestStringEntity.Data))).Returns(pageRequest);
            resolveMod.Setup(p => p.Resolve<IDataSerializer>()).Returns(defaultSerial);
            resolveMod.Setup(p => p.Resolve<IValidRequest>()).Returns(validRequestMod.Object);
            ((ICommand)query).Resolve = resolveMod.Object;
            // Act
            var result = query.Run(requestStringEntity);
        
            // Assert
            Assert.NotNull(result);
            var actResult = defaultSerial.Deserialize<PageResult<Data,SummaryData>>(result);
            Assert.IsInstanceOf<PageResult<Data>>(actResult);
            Assert.NotNull(actResult);
            Assert.That(actResult.TotalCount, Is.EqualTo(100));
            
            Assert.That(actResult.PageData.Count, Is.EqualTo(10));
            Assert.NotNull(actResult.Summary);
            Assert.That(actResult.Summary.SummaryIndex, Is.EqualTo(100));
        }
        //
        // [Test]
        // public void PageQueryBase_With_Query_Parameters_Should_Return_PageResult_With_Summary_Data()
        // {
        //     // Arrange
        //     var query = new PageQueryBaseImplWithParams();
        //
        //     // Act
        //     var result = query.ExecuteWithSummary(new QueryParameters());
        //
        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.IsInstanceOf<PageResult<Data>>(result);
        //     Assert.NotNull(result.SummaryData);
        // }
    }
    public class PageQueryBaseImplWithNoList : PageQueryBase<PageResult<Data>, Data>
    {
        protected override PageResult<Data> CreateResult(PageRequest request)
        {
            var result = new PageResult<Data>(request);
            return result;
        }

        protected override List<Data> GetDataList(PageRequest request)
        {
            List<Data> data = new List<Data>();
            var start = request.PageIndex * request.PageSize + 1;
            for (int i = 0; i < request.PageSize; i++)
            {
                data.Add(new Data(){Index = start+i});
            }
            return data;
        }

        public override long GetTotalCount(PageRequest request)
        {
            return 0;
        }
    }
    public class PageQueryBaseImplWithList : PageQueryBase<PageResult<Data>, Data>
    {
        protected override PageResult<Data> CreateResult(PageRequest request)
        {
            var result = new PageResult<Data>(request);
            return result;
        }

        protected override List<Data> GetDataList(PageRequest request)
        {
            List<Data> data = new List<Data>();
            var start = request.PageIndex * request.PageSize + 1;
            for (int i = 0; i < request.PageSize; i++)
            {
                data.Add(new Data(){Index = start+i});
            }
            return data;
        }

        public override long GetTotalCount(PageRequest request)
        {
            return 30;
        }
    }
    // Implementations for testing
    public class PageQueryBaseImplWithSummary : PageQuerySummaryBase<PageResult<Data,SummaryData>, Data, SummaryData>
    {
        protected override PageResult<Data, SummaryData> CreateResult(PageRequest request)
        {
            return new PageResult<Data, SummaryData>(request);
        }

        protected override List<Data> GetDataList(PageRequest request)
        {
            List<Data> data = new List<Data>();
            var start = request.PageIndex * request.PageSize + 1;
            for (int i = 0; i < request.PageSize; i++)
            {
                data.Add(new Data(){Index = start+i});
            }
            return data;
        }

        public override long GetTotalCount(PageRequest request)
        {
            return 100;
        }

        protected override SummaryData GetSummaryData(PageRequest request)
        {
            return new SummaryData() { SummaryIndex = 100};
        }
    }

    public class PageQueryBaseImplWithParams : PageQueryBase<QueryParameters, PageResult<Data>, Data>
    {
        protected override PageResult<Data> CreateResult(PageRequest<QueryParameters> request)
        {
            throw new NotImplementedException();
        }

        protected override List<Data> GetDataList(PageRequest request)
        {
            throw new NotImplementedException();
        }

        public override long GetTotalCount(PageRequest request)
        {
            throw new NotImplementedException();
        }
    }

    public class SummaryData
    {
        public int SummaryIndex { get; set; }
    }

    public class Data
    {
        public int Index { get; set; }
    }

    public class QueryParameters
    {
    }
}