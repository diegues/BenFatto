using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Api_UploadFileLog.Controllers;
using Api_UploadFileLog.Entidades;
using Api_UploadFileLog.Repository;
using Microsoft.AspNetCore.Mvc;
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

        private LogController CreateTestSubject()
        {
            return new LogController(_logRepositoryMock.Object);
        }

        #region insert
        [TestMethod]
        public void Insert_DeveCriarLogObjetoCorretamente_QuandoNaoHaErrosDeValidacao()
        {
            LogController logController = this.CreateTestSubject();

            logController.Insert(
                "127.0.0.1",
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
                l.ip == "127.0.0.1" &&
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

            IActionResult resultado = logController.Insert(
                "127.0.0.1",
                "local",
                "usuario",
                "data invalida",
                "zone",
                "requisicao",
                "0",
                "0",
                "origem",
                "software");

            Assert.AreEqual(new ObjectResult("Data inválida!").ToString(), resultado.ToString());

            _logRepositoryMock.Verify(m =>
                m.Add(It.Is<Log>(l => l.status == null)),
                Times.Never());
        }
        public void Insert_DeveRetornarMensagemDeIpInvalido_QuandoIpEInvalido()
        {
            LogController logController = this.CreateTestSubject();

            IActionResult resultado = logController.Insert(
                "ip inválido",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "0",
                "0",
                "origem",
                "software");

            Assert.AreEqual(new ObjectResult("Ip inválido!").ToString(), resultado.ToString());

            _logRepositoryMock.Verify(m =>
                m.Add(It.Is<Log>(l => l.status == null)),
                Times.Never());
        }

        [TestMethod]
        public void Insert_DeveInserirStatusNulo_QuandoStatusEInvalido()
        {
            LogController logController = this.CreateTestSubject();

            IActionResult resultado = logController.Insert(
                "127.0.0.1",
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

            IActionResult IActionResult = logController.Insert(
                "127.0.0.1",
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

            IActionResult resultado = logController.Insert(
                "127.0.0.1",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "10",
                "10",
                "origem",
                "software");

            Assert.AreEqual(new ObjectResult("Inserido com sucesso.").ToString(), resultado.ToString());
        }

        [TestMethod]
        public void Insert_DeveRetornarErrorAoInserirDados_QuandoLogNaoFoiInserido()
        {
            _logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>())).Returns(0);

            LogController logController = this.CreateTestSubject();

            IActionResult resultado = logController.Insert(
                "127.0.0.1",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "10",
                "10",
                "origem",
                "software");

            Assert.AreEqual(new ObjectResult("Erro ao inserir dados.").ToString(), resultado.ToString());
        }

        [TestMethod]
        public void Insert_DeveRetornarExcecao_QuandoInesperadaExcecaoAcontecer()
        {
            _logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>())).Throws(new Exception("Inserir falhou."));

            LogController logController = this.CreateTestSubject();

            Assert.ThrowsException<Exception>(() => logController.Insert(
                "127.0.0.1",
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

            IActionResult resultado = logController.Insert(
                "127.0.0.1",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "0",
                "0",
                "origem",
                "software");

            Assert.AreEqual(new ObjectResult(mensageEsperada).ToString(), resultado.ToString());

            _logRepositoryMock.Verify(m =>
                m.Add(It.IsAny<Log>()),
                Times.Once());
        }
        #endregion

        #region CreateLog
        [TestMethod]
        public void PostCreateLog_DeveCriarLogObjetoCorretamente_QuandoNaoHaErrosDeValidacao()
        {
            LogModel modelEnvio = new LogModel();
            modelEnvio.ip = "127.0.0.1";
            modelEnvio.local = "local";
            modelEnvio.usuario = "usuario";
            modelEnvio.data = "24/Jan/2021 16:00:00";
            modelEnvio.zone = "zone";
            modelEnvio.requisicao = "requisicao";
            modelEnvio.status = "10";
            modelEnvio.time = "10";
            modelEnvio.origem = "origem";
            modelEnvio.software = "software";

            LogController logController = this.CreateTestSubject();

            IActionResult resultado = logController.PostCreateLog(modelEnvio);

            _logRepositoryMock.Verify(m =>
                m.Add(It.Is<Log>(l =>
                l.ip == "127.0.0.1" &&
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
        public void PostCreateLog_DeveRetornarMensagemDeDataInvalida_QuandoDataEInvalida()
        {
            LogController logController = this.CreateTestSubject();

            LogModel modelEnvio = new LogModel();
            modelEnvio.ip = "127.0.0.1";
            modelEnvio.local = "local";
            modelEnvio.usuario = "usuario";
            modelEnvio.data = "data invalida";
            modelEnvio.zone = "zone";
            modelEnvio.requisicao = "requisicao";
            modelEnvio.status = "0";
            modelEnvio.time = "0";
            modelEnvio.origem = "origem";
            modelEnvio.software = "software";

            IActionResult resultado = logController.PostCreateLog(modelEnvio);

            Assert.AreEqual(new ObjectResult("Data inválida!").ToString(), resultado.ToString());

            _logRepositoryMock.Verify(m =>
                m.Add(It.Is<Log>(l => l.status == null)),
                Times.Never());
        }

        [TestMethod]
        public void PostCreateLog_DeveInserirStatusNulo_QuandoStatusEInvalido()
        {
            LogController logController = this.CreateTestSubject();

            LogModel modelEnvio = new LogModel();
            modelEnvio.ip = "127.0.0.1";
            modelEnvio.local = "local";
            modelEnvio.usuario = "usuario";
            modelEnvio.data = "24/Jan/2021 16:00:00";
            modelEnvio.zone = "zone";
            modelEnvio.requisicao = "requisicao";
            modelEnvio.status = "status invalido";
            modelEnvio.time = "10";
            modelEnvio.origem = "origem";

            IActionResult resultado = logController.PostCreateLog(modelEnvio);

            _logRepositoryMock.Verify(m =>
                m.Add(It.Is<Log>(l => l.status == null)),
                Times.Once());
        }

        [TestMethod]
        public void PostCreateLog_DeveInserirTimeNulo_QuandoTimeEInvalido()
        {
            LogController logController = this.CreateTestSubject();

            LogModel modelEnvio = new LogModel();
            modelEnvio.ip = "127.0.0.1";
            modelEnvio.local = "local";
            modelEnvio.usuario = "usuario";
            modelEnvio.data = "24/Jan/2021 16:00:00";
            modelEnvio.zone = "zone";
            modelEnvio.requisicao = "requisicao";
            modelEnvio.status = "10";
            modelEnvio.time = "time invalido";
            modelEnvio.origem = "origem";

            IActionResult resultado = logController.PostCreateLog(modelEnvio);

            _logRepositoryMock.Verify(m =>
                m.Add(It.Is<Log>(l => l.time == null)),
                Times.Once());
        }

        [TestMethod]
        public void PostCreateLog_DeveRetornarInseridoComSucesso_QuandoLogFoiInseridoCorretamente()
        {
            LogController logController = this.CreateTestSubject();
            _logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>())).Returns(1);

            LogModel modelEnvio = new LogModel();
            modelEnvio.ip = "127.0.0.1";
            modelEnvio.local = "local";
            modelEnvio.usuario = "usuario";
            modelEnvio.data = "24/Jan/2021 16:00:00";
            modelEnvio.zone = "zone";
            modelEnvio.requisicao = "requisicao";
            modelEnvio.status = "0";
            modelEnvio.time = "0";
            modelEnvio.origem = "origem";
            modelEnvio.software = "software";

            IActionResult resultado = logController.PostCreateLog(modelEnvio);

            Assert.AreEqual(new ObjectResult("Inserido com sucesso.").ToString(), resultado.ToString());
        }

        [TestMethod]
        public void PostCreateLog_DeveRetornarErrorAoInserirDados_QuandoLogNaoFoiInserido()
        {
            _logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>())).Returns(0);

            LogController logController = this.CreateTestSubject();

            LogModel modelEnvio = new LogModel();
            modelEnvio.ip = "127.0.0.1";
            modelEnvio.local = "local";
            modelEnvio.usuario = "usuario";
            modelEnvio.data = "24/Jan/2021 16:00:00";
            modelEnvio.zone = "zone";
            modelEnvio.requisicao = "requisicao";
            modelEnvio.status = "0";
            modelEnvio.time = "0";
            modelEnvio.origem = "origem";
            modelEnvio.software = "software";

            IActionResult resultado = logController.PostCreateLog(modelEnvio);

            Assert.AreEqual(new ObjectResult("Erro ao inserir dados.").ToString(), resultado.ToString());
        }

        [TestMethod]
        public void PostCreateLog_DeveRetornarExcecao_QuandoInesperadaExcecaoAcontecer()
        {
            _logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>())).Throws(new Exception("Inserir falhou."));

            LogController logController = this.CreateTestSubject();

            LogModel modelEnvio = new LogModel();
            modelEnvio.ip = "127.0.0.1";
            modelEnvio.local = "local";
            modelEnvio.usuario = "usuario";
            modelEnvio.data = "24/Jan/2021 16:00:00";
            modelEnvio.zone = "zone";
            modelEnvio.requisicao = "requisicao";
            modelEnvio.status = "10";
            modelEnvio.time = "10";
            modelEnvio.origem = "origem";
            modelEnvio.software = "software";

            Assert.ThrowsException<Exception>(() => logController.PostCreateLog(modelEnvio));

            _logRepositoryMock.Verify(m =>
                m.Add(It.IsAny<Log>()),
                Times.Once());
        }

        [TestMethod]
        public void PostCreateLog_DeveExcecaoDeDados_QuandoExcecaoDeDadosAcontecer()
        {
            string mensageEsperada = "qualquer mensagem";
            _logRepositoryMock.Setup(m => m.Add(It.IsAny<Log>())).Throws(new DataException(mensageEsperada));

            LogController logController = this.CreateTestSubject();

            LogModel modelEnvio = new LogModel();
            modelEnvio.ip = "127.0.0.1";
            modelEnvio.local = "local";
            modelEnvio.usuario = "usuario";
            modelEnvio.data = "24/Jan/2021 16:00:00";
            modelEnvio.zone = "zone";
            modelEnvio.requisicao = "requisicao";
            modelEnvio.status = "0";
            modelEnvio.time = "0";
            modelEnvio.origem = "origem";
            modelEnvio.software = "software";

            IActionResult resultado = logController.PostCreateLog(modelEnvio);

            Assert.AreEqual(new ObjectResult(mensageEsperada).ToString(), resultado.ToString());

            _logRepositoryMock.Verify(m =>
                m.Add(It.IsAny<Log>()),
                Times.Once());
        }

        #endregion


        #region SelectFields
        [TestMethod]
        public void SelectFields_DeveRetornarResultado_QuandoSelectAcontecerSemErros()
        {
            List<LogModel> lstEnvio = new List<LogModel>();
            lstEnvio.Add(new LogModel());

            _logRepositoryMock.Setup(m => m.SelectWithParameters(It.IsAny<Log>())).Returns(lstEnvio);

            LogController logController = this.CreateTestSubject();

            string resultadoEsperado = JsonSerializer.Serialize(lstEnvio, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            });

            IActionResult resultado = logController.SelectFields(
                "0",
                "127.0.0.1",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "0",
                "0",
                "origem",
                "software");

            Assert.AreEqual(new ObjectResult(resultadoEsperado).ToString(), resultado.ToString());

            _logRepositoryMock.Verify(m =>
                m.SelectWithParameters(It.IsAny<Log>()),
                Times.Once());
        }

        [TestMethod]
        public void SelectFields_DeveRetornarIpInvalido_QuandoIdForInvalido()
        {
            LogController logController = this.CreateTestSubject();
            string mensagemEsperada = "Ip inválido!";

            IActionResult resultado = logController.SelectFields(
                "0",
                "ip inválido",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "0",
                "0",
                "origem",
                "software");

            Assert.AreEqual(new ObjectResult(mensagemEsperada).ToString(), resultado.ToString());

        }

        [TestMethod]
        public void SelectFields_DeveRetornarSemDadosParaExibir_QuandoNaoHouverDados()
        {
            _logRepositoryMock.Setup(m => m.SelectWithParameters(It.IsAny<Log>())).Returns(It.IsAny<List<LogModel>>());

            LogController logController = this.CreateTestSubject();
            string mensagemEsperada = "Sem dados para exibir!";

            IActionResult resultado = logController.SelectFields(
                "0",
                "127.0.0.1",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "0",
                "0",
                "origem",
                "software");

            Assert.AreEqual(new ObjectResult(mensagemEsperada).ToString(), resultado.ToString());

        }


        [TestMethod]
        public void SelectFields_DeveExcecaoDeDados_QuandoExcecaoDeDadosAcontecer()
        {
            string mensageEsperada = "qualquer mensagem";
            _logRepositoryMock.Setup(m => m.SelectWithParameters(It.IsAny<Log>())).Throws(new Exception(mensageEsperada));

            LogController logController = this.CreateTestSubject();

            IActionResult resultado = logController.SelectFields(
                "0",
                "127.0.0.1",
                "local",
                "usuario",
                "24/Jan/2021 16:00:00",
                "zone",
                "requisicao",
                "0",
                "0",
                "origem",
                "software");

            Assert.AreEqual(new ObjectResult(mensageEsperada).ToString(), resultado.ToString());

            _logRepositoryMock.Verify(m =>
                m.SelectWithParameters(It.IsAny<Log>()),
                Times.Once());
        }
        #endregion
    }

}
