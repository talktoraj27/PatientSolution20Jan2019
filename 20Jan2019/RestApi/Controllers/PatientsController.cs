using System;
using System.Net;
using System.Web.Http;
using RestApi.Interfaces;
using RestApi.Models;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;

namespace RestApi.Controllers
{
    public class PatientsController : ApiController
    {

        #region Private members

        private IDatabaseContext _context = default(IDatabaseContext);

        #endregion

        #region Constructors

        public PatientsController(IDatabaseContext context)
        {
            _context = context;
        }

        #endregion

        #region Actions

        [HttpGet]
        public HttpResponseMessage Get(int patientId)
        {
            var returnValue = null as HttpResponseMessage;

            try
            {
                if (patientId <= 0)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                    //returnValue = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                else
                {
                    var patientsWithEpisodes = (from p in _context.Patients
                                                join e in _context.Episodes on p.PatientId equals e.PatientId
                                                where p.PatientId == patientId
                                                select new { p, e });

                    var data = null as Patient;

                    if (patientsWithEpisodes == null)
                    {
                        returnValue = Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    else if (patientsWithEpisodes.Any())
                    {
                        data = patientsWithEpisodes.First().p;
                        data.Episodes = patientsWithEpisodes.Select(x => x.e).ToList();

                        returnValue = Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return returnValue;
        }

        #endregion

    }
}