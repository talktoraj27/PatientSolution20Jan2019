using System;
using System.Collections.Generic;
using System.Data.Entity;
using RestApi.Interfaces;

namespace RestApi.Models
{
    public class InMemoryPatientContext : IDatabaseContext
    {
        public IDbSet<Patient> Patients { get; set; }
        public IDbSet<Episode> Episodes { get; set; }

        public InMemoryPatientContext()
        {
            Episodes = new InMemoryDbSet<Episode>();
            Patients = new InMemoryDbSet<Patient>();

            PopulateDbSets();
        }

        private void PopulateDbSets()
        {
            Patients.Add(
                new Patient
                {
                    DateOfBirth = new DateTime(1972, 10, 27),
                    FirstName = "Brett",
                    PatientId = 111,
                    LastName = "Lee",
                    NhsNumber = "NHS12345"
                });

            Episodes.Add(
                new Episode
                {
                    AdmissionDate = new DateTime(2019, 01, 01),
                    Diagnosis = "Tennis Elbow",
                    DischargeDate = new DateTime(2019, 01, 15),
                    EpisodeId = 999,
                    PatientId = 111
                });
        }

    }
}