using Api_UploadFileLog.Entidades;
using Api_UploadFileLog.Repository;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;

namespace Api_UploadFileLog.Tests.Repository
{
    [TestClass]
    public class LogRepositoryTests
    {
        private readonly Mock<IDbConnectionWrapper> _connMock = new Mock<IDbConnectionWrapper>();

        private LogRepository CreateTestSubject()
        {
            return new LogRepository(_connMock.Object);
        }

        #region Add
        [TestMethod]
        public void Add_DeveAbrirConnexaoComBancoDeDados_QuandoConexaoEstaFechada()
        {
            _connMock.Setup(m => m.State).Returns(ConnectionState.Closed);

            LogRepository repository = this.CreateTestSubject();

            repository.Add(new Log());

            _connMock.Verify(mocks => mocks.Open(), Times.Once());
        }

        [TestMethod]
        public void Add_NaoDeveAbrirConnexaoComBancoDeDados_QuandoConexaoEstaAberta()
        {
            _connMock.Setup(m => m.State).Returns(ConnectionState.Open);

            LogRepository repository = this.CreateTestSubject();

            repository.Add(new Log());

            _connMock.Verify(mocks => mocks.Open(), Times.Never());
        }

        [TestMethod]
        public void Add_ExecutarComandoDeInsertComParametrosCorretos_QuandoFuncaoEChamada()
        {
            LogRepository repository = this.CreateTestSubject();

            Log logEsperado = new Log { ip = "127.0.0.1",
                local = "local",
                usuario= "usuario",
                data = Convert.ToDateTime("24/Jan/2021 16:00:00"),
                zone = "zone",
                requisicao = "requisicao",
                status = 10,
                time = 10,
                origem = "origem",
                software = "software" };

            repository.Add(logEsperado);

            _connMock.Verify(mocks => 
                mocks.Execute(
                    It.IsAny<string>(),
                    It.Is<Log>(x => x.ip == logEsperado.ip &&
                                x.local == logEsperado.local &&
                                x.usuario == logEsperado.usuario &&
                                x.data == logEsperado.data &&
                                x.zone == logEsperado.zone &&
                                x.requisicao == logEsperado.requisicao &&
                                x.status == logEsperado.status &&
                                x.time == logEsperado.time &&
                                x.origem == logEsperado.origem &&
                                x.software == logEsperado.software)), 
                Times.Once());
        }

        [TestMethod]
        public void Add_ConexaoComBancoDeDadosDeveSerFechado_QuandoOperacaoEExecutada()
        {
            LogRepository repository = this.CreateTestSubject();

            repository.Add(new Log());

            _connMock.Verify(mocks => mocks.Close(), Times.Once());
        }

        [TestMethod]
        public void Add_RetornarDadosExcecao_QuandoHouverFalhaNaExecucao()
        {
            _connMock.Setup(mocks => mocks.Execute(It.IsAny<string>(), It.IsAny<Log>())).Throws(new Exception());

           LogRepository repository = this.CreateTestSubject();

           Assert.ThrowsException<DataException>(() => repository.Add(new Log()));
        }
        #endregion

        #region AddList
        [TestMethod]
        public void AddList_DeveAbrirConnexaoComBancoDeDados_QuandoConexaoEstaFechada()
        {
            _connMock.Setup(m => m.State).Returns(ConnectionState.Closed);

            LogRepository repository = this.CreateTestSubject();

            repository.AddList(new List<Log>());

            _connMock.Verify(mocks => mocks.Open(), Times.Once());
        }

        [TestMethod]
        public void AddList_NaoDeveAbrirConnexaoComBancoDeDados_QuandoConexaoEstaAberta()
        {
            _connMock.Setup(m => m.State).Returns(ConnectionState.Open);

            LogRepository repository = this.CreateTestSubject();

            repository.AddList(new List<Log>());

            _connMock.Verify(mocks => mocks.Open(), Times.Never());
        }

        [TestMethod]
        public void AddList_ExecutarComandoDeInsertComParametrosCorretos_QuandoFuncaoEChamada()
        {
            LogRepository repository = this.CreateTestSubject();

            _connMock.Setup(m => m.Execute(It.IsAny<string>(),It.IsAny<object>())).Returns(1);

            List<Log> lstLog = new List<Log>();
            Log logTeste = new Log
            {
                ip = "127.0.0.1",
                local = "local",
                usuario = "usuario",
                data = Convert.ToDateTime("24/Jan/2021 16:00:00"),
                zone = "zone",
                requisicao = "requisicao",
                status = 10,
                time = 10,
                origem = "origem",
                software = "software"
            };
            lstLog.Add(logTeste);

            int resultado = repository.AddList(lstLog);

            Assert.AreEqual(resultado > 0, true);
        }

        [TestMethod]
        public void AddList_ConexaoComBancoDeDadosDeveSerFechado_QuandoOperacaoEExecutada()
        {
            LogRepository repository = this.CreateTestSubject();

            repository.AddList(new List<Log>());

            _connMock.Verify(mocks => mocks.Close(), Times.Once());
        }

        [TestMethod]
        public void AddList_RetornarDadosExcecao_QuandoHouverFalhaNaExecucao()
        {
            _connMock.Setup(mocks => mocks.Execute(It.IsAny<string>(), It.IsAny<List<Log>>())).Throws(new Exception());

            LogRepository repository = this.CreateTestSubject();

            int resultado = repository.AddList(It.IsAny<List<Log>>());

            Assert.AreEqual(resultado == 0, true);
        }
        #endregion

        #region SelectWithParameters
        [TestMethod]
        public void SelectWithParameters_DeveAbrirConnexaoComBancoDeDados_QuandoConexaoEstaFechada()
        {
            _connMock.Setup(m => m.State).Returns(ConnectionState.Closed);

            LogRepository repository = this.CreateTestSubject();

            repository.SelectWithParameters(new Log());

            _connMock.Verify(mocks => mocks.Open(), Times.Once());
        }

        [TestMethod]
        public void SelectWithParameters_NaoDeveAbrirConnexaoComBancoDeDados_QuandoConexaoEstaAberta()
        {
            _connMock.Setup(m => m.State).Returns(ConnectionState.Open);

            LogRepository repository = this.CreateTestSubject();

            repository.SelectWithParameters(new Log());

            _connMock.Verify(mocks => mocks.Open(), Times.Never());
        }

        [TestMethod]
        public void SelectWithParameters_ExecutarComandoDeSelect_QuandoFuncaoEChamada()
        {
            LogRepository repository = this.CreateTestSubject();

            LogModel resultadoEsperado = new LogModel
            {
                ip = "127.0.0.1",
                local = "local",
                usuario = "usuario",
                data = "24/Jan/2021 16:00:00",
                zone = "zone",
                requisicao = "requisicao",
                status = "10",
                time = "10",
                origem = "origem",
                software = "software"
            };
            List<LogModel> envio = new List<LogModel>();
            envio.Add(resultadoEsperado);

            List<LogModel> resultadoRetorno = new List<LogModel>();
            resultadoRetorno.Add(resultadoEsperado);

            _connMock.Setup(m => m.Query<LogModel>(It.IsAny<string>(), It.IsAny<Log>())).Returns(envio);

            List<LogModel> resultado = repository.SelectWithParameters(new Log());

            string JsonRetornado = JsonSerializer.Serialize(resultado);
            string JsonRetornoEsperado = JsonSerializer.Serialize(resultadoRetorno);


            Assert.AreEqual(JsonRetornoEsperado, JsonRetornado);
        }

        [TestMethod]
        public void SelectWithParameters_ConexaoComBancoDeDadosDeveSerFechado_QuandoOperacaoEExecutada()
        {
            LogRepository repository = this.CreateTestSubject();

            repository.SelectWithParameters(new Log());

            _connMock.Verify(mocks => mocks.Close(), Times.Once());
        }

        #endregion
    }
}