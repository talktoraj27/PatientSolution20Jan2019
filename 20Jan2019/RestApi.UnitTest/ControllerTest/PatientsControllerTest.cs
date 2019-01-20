using System;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestApi.Controllers;
using RestApi.DependencyInjection;
using RestApi.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Hosting;
using Newtonsoft.Json;
using System.Net.Http;

namespace RestApi.UnitTest.ControllerTest
{
    [TestClass]
    public class PatientsControllerTest
    {
        [TestMethod]
        public void GetPatientWithEpisodesWhenPatientExists()
        {
            // Arrange
            int patientId = 111;
            var container = Dependency.SetDependencies(true);
            var patientsController = container.Resolve<PatientsController>();
            patientsController.Request = new HttpRequestMessage();
            patientsController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = new HttpConfiguration();


            var expected = new Patient
            {
                DateOfBirth = new DateTime(1972, 10, 27),
                FirstName = "Brett",
                PatientId = patientId,
                LastName = "Lee",
                NhsNumber = "NHS12345",
                Episodes = new List<Episode>
                {
                    new Episode
                    {
                        AdmissionDate = new DateTime(2019, 01, 01),
                        Diagnosis = "Tennis Elbow",
                        DischargeDate = new DateTime(2019, 01, 15),
                        EpisodeId = 999,
                        PatientId = patientId
                    }
                }
            };

            // Act
            var actual = patientsController.Get(patientId);
            bool isTrue = actual.TryGetContentValue(out Patient patient);


            // assert
            Assert.IsTrue(isTrue);
            Assert.AreEqual(expected.FirstName, patient.FirstName);
            Assert.AreEqual(expected.LastName, patient.LastName);
            Assert.AreEqual(expected.NhsNumber, patient.NhsNumber);

            var actualDiagnosis = "";
            foreach (var episode in patient.Episodes)
            {
                actualDiagnosis = episode.Diagnosis;
            }

            Assert.AreEqual("Tennis Elbow", actualDiagnosis);

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetIfPatientIdIsInValidThenThrowsHttpResponseException()
        {
            // Arrange
            int patientId = -222;
            var container = Dependency.SetDependencies(true);
            var patientsController = container.Resolve<PatientsController>();
            patientsController.Request = new HttpRequestMessage();
            patientsController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = new HttpConfiguration();

            // act
            var actual = patientsController.Get(patientId);
        }

    }
}
