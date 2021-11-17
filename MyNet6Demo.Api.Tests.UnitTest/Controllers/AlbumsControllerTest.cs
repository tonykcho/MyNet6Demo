using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyNet6Demo.Api.Controllers;
using MyNet6Demo.Domain.Exceptions;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Api.Tests.UnitTest
{
    [TestClass]
    public class AlbumsControllerTest
    {
        // public event Action DoSomething;

        // [TestMethod]
        // public async Task GetAlbumByIdAsync_Album_Expect200()
        // {
        //     DoSomething += () => { Console.WriteLine("Hello"); };

        //     Action worldAction = () => { Console.WriteLine("World"); };

        //     DoSomething += worldAction;

        //     DoSomething.Invoke();

        //     DoSomething -= worldAction;

        //     DoSomething.Invoke();

        //     CancellationTokenSource source = new CancellationTokenSource();

        //     var mockAlbumRepository = new Mock<IAlbumRepository>();

        //     mockAlbumRepository.Setup(repository => repository.get)

        //     Assert.Fail();
        // }

        [TestMethod]
        public async Task GetAlbumByIdAsync_ReturnNull_Expect404()
        {
            CancellationTokenSource source = new CancellationTokenSource();

            var mockAlbumRepository = new Mock<IAlbumRepository>();

            mockAlbumRepository.Setup(repository => repository.GetByIdAsync(It.IsAny<int>(), source.Token))
                .Returns(Task.FromResult<Album?>(null));

            var controller = new AlbumsController(mockAlbumRepository.Object);

            await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () => {
                await controller.GetAlbumByIdAsync(1, source.Token);
            });

            // var actionResult = await controller.GetAlbumByIdAsync(1, source.Token);

            // Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));

            // var contentResult = actionResult as NotFoundResult;

            // Assert.AreEqual(404, contentResult?.StatusCode);
        }
    }
}