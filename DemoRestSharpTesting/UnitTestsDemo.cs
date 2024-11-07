using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace DemoRestSharpTesting
{
    public class UnitTestsDemo
    {
        RestClient client;
        private Issue CreateIssue(string title, string body)
        {
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues");
            request.AddBody(new {body, title});
            var response = client.Execute(request, Method.Post);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            return issue;

        }
        [SetUp]
        public void Setup()
        {
            var options = new RestClientOptions("https://api.github.com")
            {
                MaxTimeout = 3000,
                Authenticator = new HttpBasicAuthenticator("Dzhantov", "TOKEN")
            };
            this.client = new RestClient(options);
        }

        [Test]
        public void Test_GitHubAPIRequest()
        {
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Get);
            var response = client.Get(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void CreateNewIssue()
        {
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues");
            request.AddBody(new { title = "SomeRandomtitleDzhantov",body = "randoom body777" });
           
            var response = client.Post(request);

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.That(issue.id, Is.GreaterThan(0));
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.EqualTo("SomeRandomtitleDzhantov"));
            Assert.That(issue.body, Is.EqualTo("randoom body777"));

        }

        [Test]
        public void CTest_CreateIssue()
        {
            string title = "titleDzhantov1";
            string body = "Dzhantov test 1";
            var issue = CreateIssue(title, body);
            Assert.That(issue.id, Is.GreaterThan(0));
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.Not.Empty);
        }

        [Test]

        public void Test_EditCreatedIssue()
        {
            var reqeust = new RestRequest("repos/testnakov/test-nakov-repo/issues/8002");
            reqeust.AddBody(new {
                title = "new title Dzhantov" 
            }
            );

            var response = client.Execute(reqeust, Method.Patch);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(issue.id, Is.GreaterThan(0));
            Assert.That(response.Content, Is.Not.Empty);
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.EqualTo("new title Dzhantov"));

        }
    }
}