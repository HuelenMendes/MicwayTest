using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Micway.Models;

namespace Micway.Controllers
{

    [RoutePrefix("api/drivers")]
    public class DriversController : ApiController
    {
        private MicwayEntities db = new MicwayEntities();

        // GET: api/Drivers
        /// <summary>
        /// List: Returns a list of the drivers with id, fullName and email
        /// </summary>
        /// <returns></returns>         
        [ResponseType(typeof(DriverDetails[]))]
        [ActionName("List")]
        public async Task<IHttpActionResult> Get()
        {
            object[] drivers;
            try
            {
                drivers = await db.Drivers.Select(p => new DriverDetails()
                {
                    ID = p.ID,
                    FullName = (p.FirstName + " " + p.LastName),
                    Email = p.Email
                }).ToArrayAsync();

            }
            catch (Exception)
            {
                var message = "System has failed, please try again later.";
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message));
            }
            return Ok(drivers);
        }

        // GET: api/Drivers/5
        /// <summary>
        /// Details: Returns the full data of a specific driver
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Driver))]
        [ActionName("Details")]
        public async Task<IHttpActionResult> Get(int id)
        {
            Driver driver;
            try
            {
                driver = await db.Drivers.FindAsync(id);               
            }
            catch (Exception)
            {
                var message = "System has failed, please try again later.";
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message));
            }

            if (driver == null)
            {
                var message = string.Format("Driver with ID = {0} not found", id);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }

            return Ok(driver);
        }

        // PUT: api/Drivers/5
        /// <summary>
        /// Update: Update the driver details
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        [ResponseType(typeof(Driver))]
        [ActionName("Update")]
        public async Task<IHttpActionResult> Put(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (driver == null)
            {
                var message = "Driver details is not valid";
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, message));
            }

            if (!DriverExists(driver.ID))
            {
                var message = string.Format("Driver with ID = {0} not found", driver.ID);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            
            try
            {
                db.Entry(driver).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var message = "Database has failed, please try again later";
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message));
            }
            catch (Exception)
            {
                var message = "System has failed, please try again later.";
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message));
            }

            return Ok();
        }

        // POST: api/Drivers
        /// <summary>
        /// Insert: Add a new driver
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        [ResponseType(typeof(Driver))]
        [ActionName("Insert")]
        public async Task<IHttpActionResult> Post(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (driver == null)
            {
                var message = "Driver details is not valid";
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, message));
            }

            try
            {
                db.Drivers.Add(driver);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var message = "Database has failed, please try again later";
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message));
            }
            catch (Exception)
            {
                var message = "System has failed, please try again later.";
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message));
            }

            return Ok();
        }

        // DELETE: api/Drivers/5
        /// <summary>
        /// Delete: Delete a driver
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Driver))]
        [ActionName("Delete")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                var message = string.Format("Driver with ID = {0} not found", id);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }

            try
            {
                db.Drivers.Remove(driver);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var message = "Database has failed, please try again later";
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message));
            }
            catch (Exception)
            {
                var message = "System has failed, please try again later.";
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message));
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DriverExists(int id)
        {
            return db.Drivers.Count(e => e.ID == id) > 0;
        }
    }
}