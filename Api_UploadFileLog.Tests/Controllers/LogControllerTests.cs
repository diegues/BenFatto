using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Api_UploadFileLog.Controllers;
using Api_UploadFileLog.Entidades;
using Api_UploadFileLog.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Api_UploadFileLog.Tests.Controllers
{
    [TestClass]
    public class LogControllerTests
    {
        private readonly Mock<ILogRepository> _logRepositoryMock = new Mock<ILogRepository>();

        public LogControllerTests()
        {
        }

        [TestMethod]
        public void Insert_DeveCriarLogObjetoCorretamente_QuandoNaoHaErrosDeValidacao()
        {
            LogController logController = this.CreateTestSubject();

            logController.Insert(
                "ip",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "10",
                "10",
                "origem",
                "software");

            _logRepositoryMock.Verify(m =>
                m.Add(It.Is<Log>(l => 
                l.ip == "ip" &&
                l.local == "local" &&
                l.usuario == "usuario" &&
                l.data == DateTime.Parse("24/Jan/2021 16:00:00") &&
                l.zone == "zone" &&
                l.requisicao == "requisicao" &&
                l.status == 10 &&
                l.time == 10 &&
                l.origem == "origem" &&
                l.software == "software")),
                Times.Once());
        }

        [TestMethod]
        public void Insert_DeveRetornarMensagemDeDataInvalida_QuandoDataEInvalida()
        {
            LogController logController = this.CreateTestSubject();

            string resultado = logController.Insert(
                "ip",
                "local",
                "usuario",
                "data invalida",
                "zone",
                "requisicao",
                "0",
                "0",
                "origem",
                "software");

            Assert.AreEqual("Data inválida!", resultado);

            _logRepositoryMock.Verify(m =>
                m.Add(It.Is<Log>(l => l.status == null)),
                Times.Never());
        }

        [TestMethod]
        public void Insert_DeveInserirStatusNulo_QuandoStatusEInvalido()
        {
            LogController logController = this.CreateTestSubject();

            string resultado = logController.Insert(
                "ip",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "status invalido",
                "0",
                "origem",
                "software");

            _logRepositoryMock.Verify(m =>
                m.Add(It.Is<Log>(l => l.status == null)),
                Times.Once());
        }

        [TestMethod]
        public void Insert_DeveInserirTimeNulo_QuandoTimeEInvalido()
        {
            LogController logController = this.CreateTestSubject();

            string resultado = logController.Insert(
                "ip",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "0",
                "time invalido",
                "origem",
                "software");

            _logRepositoryMock.Verify(m =>
                m.Add(It.Is<Log>(l => l.time == null)),
                Times.Once());
        }

        [TestMethod]
        public void Insert_DeveRetornarInseridoComSucesso_QuandoLogFoiInseridoCorretamente()
        {
            _logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>())).Returns(1);

            LogController logController = this.CreateTestSubject();

            string resultado = logController.Insert(
                "ip",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "10",
                "10",
                "origem",
                "software");

            Assert.AreEqual("Inserido com sucesso.", resultado);
        }

        [TestMethod]
        public void Insert_DeveRetornarErrorAoInserirDados_QuandoLogNaoFoiInserido()
        {
            _logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>())).Returns(0);

            LogController logController = this.CreateTestSubject();

            string resultado = logController.Insert(
                "ip",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "10",
                "10",
                "origem",
                "software");

            Assert.AreEqual("Erro ao inserir dados.", resultado);
        }

        [TestMethod]
        public void Insert_DeveRetornarExcecao_QuandoInesperadaExcecaoAcontecer()
        {
            _logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>())).Throws(new Exception("Inserir falhou."));

            LogController logController = this.CreateTestSubject();

            Assert.ThrowsException<Exception>(() => logController.Insert(
                "ip",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "0",
                "0",
                "origem",
                "software"));

            _logRepositoryMock.Verify(m =>
                m.Add(It.IsAny<Log>()),
                Times.Once());
        }

        [TestMethod]
        public void Insert_DeveExcecaoDeDados_QuandoExcecaoDeDadosAcontecer()
        {
            string mensageEsperada = "qualquer mensagem";
            _logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>())).Throws(new DataException(mensageEsperada));

            LogController logController = this.CreateTestSubject();

            string resultado = logController.Insert(
                "ip",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "0",
                "0",
                "origem",
                "software");

            Assert.AreEqual(mensageEsperada, resultado);

            _logRepositoryMock.Verify(m =>
                m.Add(It.IsAny<Log>()),
                Times.Once());
        }

        private LogController CreateTestSubject()
        {
            return new LogController(_logRepositoryMock.Object);
        }
    }
}
