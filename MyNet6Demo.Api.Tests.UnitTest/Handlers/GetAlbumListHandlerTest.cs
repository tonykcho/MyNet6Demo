using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyNet6Demo.Core.Albums.Queries;
using MyNet6Demo.Core.Albums.ViewModels;
using MyNet6Demo.Core.Common;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;
using System.Linq;
using MockQueryable.Moq;

namespace MyNet6Demo.Api.Tests.UnitTest.Handlers
{
    [TestClass]
    public class GetAlbumListHandlerTest
    {
        [TestMethod]
        public async Task GetAlbumListHandler_Test_1()
        {
            CancellationTokenSource source = new CancellationTokenSource();

            GetAlbumListQuery query = new GetAlbumListQuery
            {

            };

            IEnumerable<Album> albums = new List<Album>()
            {
                new Album(){ AlbumName = "First" }
            };

            var mock = albums.AsQueryable().BuildMock();

            var mockAlbumRepository = new Mock<IAlbumRepository>();

            mockAlbumRepository.Setup(repository => repository.GetQuery())
                .Returns(mock.Object);

            IList<AlbumViewModel> views = new List<AlbumViewModel>
            {
                new AlbumViewModel(){ AlbumName = "First"}
            };

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(Mapper => Mapper.Map<IList<AlbumViewModel>>(It.IsAny<IList<Album>>()))
                .Returns(views);

            var handler = new GetAlbumListHandler(mockAlbumRepository.Object, mockMapper.Object);

            var result = await handler.Handle(query, source.Token);

            Assert.IsInstanceOfType(result, typeof(PaginatedList<AlbumViewModel>));

            Assert.AreEqual(1, result.TotalCount);

            Assert.AreEqual("First", result.Items[0].AlbumName);
        }
    }
}