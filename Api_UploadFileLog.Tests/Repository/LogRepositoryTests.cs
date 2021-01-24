using Api_UploadFileLog.Entidades;
using Api_UploadFileLog.Repository;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Api_UploadFileLog.Tests.Repository
{
    [TestClass]
    public class LogRepositoryTests
    {
        private readonly Mock<IDbConnectionWrapper> _connMock = new Mock<IDbConnectionWrapper>();

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

            Log logEsperado = new Log { ip = "123" };
            repository.Add(logEsperado);

            _connMock.Verify(mocks => 
                mocks.Execute(
                    It.IsAny<string>(),
                    It.Is<Log>(x => x.ip == logEsperado.ip)), 
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
        public void Add_RetornarDataExecao_QuandoHouverFalhaNaExecucao()
        {
            _connMock.Setup(mocks => mocks.Execute(It.IsAny<string>(), It.IsAny<Log>())).Throws(new Exception());

           LogRepository repository = this.CreateTestSubject();

           Assert.ThrowsException<DataException>(() => repository.Add(new Log()));
        }

        private LogRepository CreateTestSubject()
        {
            return new LogRepository(_connMock.Object);
        }
    }
}