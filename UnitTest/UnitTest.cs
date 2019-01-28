using System;
using System.Linq;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Models;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        //It has been developed to Run All Tests

        Driver driverTest = new Driver()
        {
            FirstName = "UnitTest",
            LastName = "Driver",
            Dob = DateTime.Now.Date,
            Email = "unittest@gmail.com"
        };
        Uri uriTest = new Uri("http://localhost:51485/api/drivers/");

        [TestMethod]
        public void TestMethod_Insert()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriTest + "Insert");

                #region driver object request = null

                Driver driverError1 = null;
                var result = client.PostAsJsonAsync("", driverError1).Result;
                string response = result.Content.ReadAsStringAsync().Result;
                Assert.AreEqual(false, result.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

                #endregion driver object request = null

                #region invalid email format

                Driver driverError2 = new Driver();
                driverError2.FirstName = "Test2";
                driverError2.LastName = "Test2";
                driverError2.Dob = DateTime.Now.Date;
                driverError2.Email = "Test2.com";

                result = client.PostAsJsonAsync("", driverError2).Result;
                response = result.Content.ReadAsStringAsync().Result;
                Assert.AreEqual(false, result.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

                #endregion invalid email format

                #region invalid firstname length

                Driver driverError3 = new Driver();
                driverError3.FirstName = "Test2Test2Test2Test2Test2Test2Test2Test2Test2Test2Test2";
                driverError3.LastName = "Test2";
                driverError3.Dob = DateTime.Now.Date;
                driverError3.Email = "Test2.com";

                result = client.PostAsJsonAsync("", driverError3).Result;
                response = result.Content.ReadAsStringAsync().Result;
                Assert.AreEqual(false, result.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

                #endregion invalid firstname length

                #region invalid lastname length

                Driver driverError4 = new Driver();
                driverError4.FirstName = "Test2";
                driverError4.LastName = "Test2Test2Test2Test2Test2Test2Test2Test2Test2Test2Test2";
                driverError4.Dob = DateTime.Now.Date;
                driverError4.Email = "Test2.com";

                result = client.PostAsJsonAsync("", driverError4).Result;
                response = result.Content.ReadAsStringAsync().Result;
                Assert.AreEqual(false, result.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

                #endregion invalid lastname length

                #region success insert driver Test

                result = client.PostAsJsonAsync("", driverTest).Result;
                response = result.Content.ReadAsStringAsync().Result;
                Assert.AreEqual(true, result.IsSuccessStatusCode);

                #endregion success insert driver Test
            }
        }

        [TestMethod]
        public void TestMethod_List()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriTest + "List");

                #region success get drivers

                var result = client.GetAsync("").Result;
                var response = result.Content.ReadAsAsync<DriverDetails[]>().Result;
                Assert.AreEqual(true, result.IsSuccessStatusCode);
                Assert.AreNotEqual(response.Length, 0);

                #endregion success get drivers 
            }
        }

        [TestMethod]
        public void TestMethod_Details()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpClient client1 = new HttpClient())
                {
                    using (HttpClient client2 = new HttpClient())
                    {
                        #region get specific driver that does not exist

                        client.BaseAddress = new Uri(uriTest + "Details/0");

                        var result0 = client.GetAsync("").Result;
                        var response0 = result0.Content.ReadAsAsync<Driver>().Result;
                        Assert.AreEqual(false, result0.IsSuccessStatusCode);
                        Assert.AreEqual(System.Net.HttpStatusCode.NotFound, result0.StatusCode);

                        #endregion get specific driver that does not exist

                        #region get specific driver that exists

                        client1.BaseAddress = new Uri(uriTest + "List");

                        var result1 = client1.GetAsync("").Result;
                        var response1 = result1.Content.ReadAsAsync<DriverDetails[]>().Result;
                        Assert.AreEqual(true, result1.IsSuccessStatusCode);
                        Assert.AreNotEqual(response1.Length, 0);

                        client2.BaseAddress = new Uri(uriTest + "Details/" + response1[0].ID);

                        var result2 = client2.GetAsync("").Result;
                        var response2 = result2.Content.ReadAsAsync<Driver>().Result;
                        Assert.AreEqual(true, result2.IsSuccessStatusCode);
                        Assert.AreEqual(response2.ID, response1[0].ID);
                        Assert.AreEqual(response2.FirstName + " " + response2.LastName, response1[0].FullName);
                        Assert.AreEqual(response2.Email, response1[0].Email);

                        #endregion get specific driver that exists
                    }
                }
            }
        }

        [TestMethod]
        public void TestMethod_Update()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriTest + "Update");

                #region update with object driver null

                Driver drivernull = null;
                var resultnull = client.PutAsJsonAsync("", drivernull).Result;
                var responsenull = resultnull.Content.ReadAsAsync<Driver>().Result;
                Assert.AreEqual(false, resultnull.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, resultnull.StatusCode);

                #endregion update with object driver null
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriTest + "Update");

                #region update driver does not exist

                Driver drivern0 = new Driver();
                drivern0.ID = 0;
                drivern0.FirstName = "Test Upload";
                drivern0.LastName = "Test Upload";
                drivern0.Dob = DateTime.Now;
                drivern0.Email = "test@upload.com";

                var result0 = client.PutAsJsonAsync("", drivern0).Result;
                var response0 = result0.Content.ReadAsAsync<Driver>().Result;
                Assert.AreEqual(false, result0.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.NotFound, result0.StatusCode);

                #endregion  update driver does not exist
            }

            using (HttpClient client2 = new HttpClient())
            {
                using (HttpClient client3 = new HttpClient())
                {
                    using (HttpClient client4 = new HttpClient())
                    {
                        #region update driver exist and compare information

                        client2.BaseAddress = new Uri(uriTest + "List");

                        var result1 = client2.GetAsync("").Result;
                        var response1 = result1.Content.ReadAsAsync<DriverDetails[]>().Result;
                        Assert.AreEqual(true, result1.IsSuccessStatusCode);
                        Assert.AreNotEqual(response1.Length, 0);
                        if (response1.Any(p => p.FullName == "UnitTest Driver"))
                        {
                            driverTest.ID = response1.FirstOrDefault(p => p.FullName == "UnitTest Driver").ID;
                        }                       
                        Driver driver1 = new Driver();
                        driver1.ID = driverTest.ID;
                        driver1.FirstName = "UnitTest";
                        driver1.LastName = "Driver Updated";
                        driver1.Dob = DateTime.Now.Date;
                        driver1.Email = "test@upload.com";

                        client3.BaseAddress = new Uri(uriTest + "Update");

                        var result2 = client3.PutAsJsonAsync("", driver1).Result;
                        var response2 = result2.Content.ReadAsAsync<Driver>().Result;
                        Assert.AreEqual(true, result2.IsSuccessStatusCode);

                        client4.BaseAddress = new Uri(uriTest + "Details/" + driver1.ID);

                        var result4 = client4.GetAsync("").Result;
                        var response4 = result4.Content.ReadAsAsync<Driver>().Result;
                        Assert.AreEqual(true, result4.IsSuccessStatusCode);                  

                        Assert.AreEqual(response4.ID, driver1.ID);
                        Assert.AreEqual(response4.FirstName, driver1.FirstName);
                        Assert.AreEqual(response4.LastName, driver1.LastName);
                        Assert.AreEqual(response4.Dob, driver1.Dob);
                        Assert.AreEqual(response4.Email, driver1.Email);

                        #endregion update driver exist and compare information
                    }
                }
            }
        }

        [TestMethod]
        public void TestMethod_Delete()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpClient client1 = new HttpClient())
                {
                    using (HttpClient client2 = new HttpClient())
                    {
                        #region delete specific driver that does not exist

                        client.BaseAddress = new Uri(uriTest + "Delete/0");

                        var result0 = client.DeleteAsync("").Result;
                        var response0 = result0.Content.ReadAsAsync<Driver>().Result;
                        Assert.AreEqual(false, result0.IsSuccessStatusCode);
                        Assert.AreEqual(System.Net.HttpStatusCode.NotFound, result0.StatusCode);

                        #endregion delete specific driver that does not exist

                        #region delete specific driver that exists

                        client1.BaseAddress = new Uri(uriTest + "List");

                        var result1 = client1.GetAsync("").Result;
                        var response1 = result1.Content.ReadAsAsync<DriverDetails[]>().Result;
                        Assert.AreEqual(true, result1.IsSuccessStatusCode);
                        Assert.AreNotEqual(response1.Length, 0);

                        if (response1.Any(p => p.FullName == "UnitTest Driver Updated"))
                        {
                            driverTest.ID = response1.FirstOrDefault(p => p.FullName == "UnitTest Driver Updated").ID;
                        }

                        client2.BaseAddress = new Uri(uriTest + "Delete/" + driverTest.ID);

                        var result2 = client2.DeleteAsync("").Result;
                        var response2 = result2.Content.ReadAsAsync<Driver>().Result;
                        Assert.AreEqual(true, result2.IsSuccessStatusCode);

                        #endregion delete specific driver that exists
                    }
                }
            }
        }
    }
}
