using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Reenbit_Task.Controllers;
using Reenbit_Task.Services;

namespace WebApiTests
{
    [TestClass]
    public class DocsControllerTests
    {
        [TestMethod]
        public async Task UploadFile_ValidFileAndUserEmail_ReturnsOk()
        {
            // Arrange
            var blobServiceMock = new Mock<IAzureBlobService>();
            var controller = new DocsController(blobServiceMock.Object);
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("sample.docx");
            fileMock.Setup(f => f.Length).Returns(10); // Adjust as needed
            var userEmail = "user@example.com";

            // Act
            var result = await controller.UploadFile(fileMock.Object, userEmail) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("File uploaded successfully.", result.Value);
        }

        [TestMethod]
        public async Task UploadFile_InvalidFile_ReturnsBadRequest()
        {
            // Arrange
            var blobServiceMock = new Mock<IAzureBlobService>();
            var controller = new DocsController(blobServiceMock.Object);
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("sample.txt"); // Not a .docx file
            var userEmail = "user@example.com";

            // Act
            var result = await controller.UploadFile(fileMock.Object, userEmail) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("Only .docx files are allowed.", result.Value);
        }

        [TestMethod]
        public async Task UploadFile_EmptyUserEmail_ReturnsBadRequest()
        {
            // Arrange
            var blobServiceMock = new Mock<IAzureBlobService>();
            var controller = new DocsController(blobServiceMock.Object);
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("sample.docx");
            var userEmail = string.Empty;

            // Act
            var result = await controller.UploadFile(fileMock.Object, userEmail) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("User email cannot be null or empty.", result.Value);
        }

        [TestMethod]
        public async Task UploadFile_NullUserEmail_ReturnsBadRequest()
        {
            // Arrange
            var blobServiceMock = new Mock<IAzureBlobService>();
            var controller = new DocsController(blobServiceMock.Object);
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("sample.docx");
            string userEmail = null;

            // Act
            var result = await controller.UploadFile(fileMock.Object, userEmail) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual("User email cannot be null or empty.", result.Value);
        }
    }
}