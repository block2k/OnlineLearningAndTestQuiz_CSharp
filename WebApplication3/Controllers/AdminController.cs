using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class AdminController : Controller
    {
        Project_CSharpEntities1 db = new Project_CSharpEntities1();

        // GET: Admin
        public ActionResult Index()
        {
            string courseId = "1";
            try
            {
                courseId = Request.QueryString["courseId"].ToString() == null ? "1" : Request["courseId"].ToString();
            }
            catch (Exception)
            {
                courseId = "1";
            }
            var ListCategory = db.Courses.SqlQuery("SELECT * FROM dbo.Course").ToList();
            var listLesson = db.Course_Lesson.SqlQuery("SELECT * FROM dbo.Course_Lesson WHERE CourseID = " + courseId).ToList();
            ViewBag.activeDashboard = "active";
            ViewBag.listLesson = listLesson;
            ViewBag.courseId = courseId;
            ViewBag.ListCategory = ListCategory;
            return View();
        }
        public ActionResult Login()
        {
            Account account = (Account)Session["account"];
            if (account != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string username = form["username"].ToString();
            string password = form["password"].ToString();
            Account account = db.Accounts.SqlQuery("SELECT * FROM dbo.Account WHERE Username = '" + username + "' AND Password = '" + password + "'").FirstOrDefault();
            if (account != null)
            {
                Session["account"] = account;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.notice = "Wrong username or password";
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
        public ActionResult UpdateLesson()
        {
            string lessonId = Request["lessonId"].ToString();
            Course_Lesson lesson = db.Course_Lesson.SqlQuery("SELECT * FROM dbo.Course_Lesson WHERE LessonID = " + lessonId).FirstOrDefault();
            var listCourse = db.Courses.SqlQuery("SELECT * FROM dbo.Course;").ToList();
            ViewBag.listCourse = listCourse;
            ViewBag.activeDashboard = "active";
            ViewBag.lesson = lesson;
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateLesson(FormCollection form)
        {
            string lName = form["lessonname"].ToString();
            string lSumary = form["summary"].ToString();
            string category = form["category"].ToString();
            string authorname = form["authorname"].ToString();
            string lessoncontent = form["lessoncontent"].ToString();
            string lessonid = form["lessonid"].ToString();
            var sql = "UPDATE [dbo].[Course_Lesson] SET " +
                "[LessonName] = {0} ," +
                "[LessonSumary] = {1} ," +
                "[LessonContent] = {2} ," +
                "[LessonImage] = 'not' ," +
                "[LessonAuthor] = {3} ," +
                "[CourseID] = {4} " +
                "WHERE LessonID = {5}";
            db.Database.ExecuteSqlCommand(sql, lName, lSumary, lessoncontent, authorname, category, lessonid);
            return RedirectToAction("Index", "Admin", new { courseId = category });
        }
        public ActionResult AddLesson()
        {
            string courseId = Request["courseId"].ToString();
            var listCourse = db.Courses.SqlQuery("SELECT * FROM dbo.Course;").ToList();
            ViewBag.courseId = courseId;
            ViewBag.activeDashboard = "active";
            ViewBag.listCourse = listCourse;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddLesson(FormCollection form)
        {
            string lName = form["lessonname"].ToString();
            string lSumary = form["summary"].ToString();
            string category = form["category"].ToString();
            string authorname = form["authorname"].ToString();
            string lessoncontent = form["lessoncontent"].ToString();
            string lessonid = form["lessonid"].ToString();
            var sql = "INSERT INTO [dbo].[Course_Lesson] " +
                "([LessonName] ," +
                "[LessonSumary] ," +
                "[LessonContent] ," +
                "[LessonAuthor] ," +
                "[CourseID]) VALUES ({0} ,{1} ,{2} ,{3} ,{4})";
            db.Database.ExecuteSqlCommand(sql, lName, lSumary, lessoncontent, authorname, category);
            return RedirectToAction("Index", "Admin", new { courseId = category });
        }
        public ActionResult DeleteLesson()
        {
            string lessonId = Request["lessonId"].ToString();
            string courseId = Request["courseId"].ToString();
            string sql = "DELETE FROM dbo.Course_Lesson WHERE LessonID = {0}";
            db.Database.ExecuteSqlCommand(sql, lessonId);
            return RedirectToAction("Index", "Admin", new { courseId = courseId });
        }
        public ActionResult AddQuiz()
        {
            var listCourse = db.Courses.SqlQuery("SELECT * FROM dbo.Course;").ToList();
            string courseId = Request["courseId"].ToString();
            ViewBag.courseId = courseId;
            ViewBag.activeManageQuiz = "active";
            ViewBag.listCourse = listCourse;
            return View();
        }
        public ActionResult UpdateQuiz()
        {
            var listCourse = db.Courses.SqlQuery("SELECT * FROM dbo.Course;").ToList();
            string quizId = Request["quizId"].ToString();
            Quiz_Bank quiz = db.Quiz_Bank.SqlQuery("SELECT * FROM dbo.Quiz_Bank WHERE QuizID = " + quizId).FirstOrDefault();
            ViewBag.quiz = quiz;
            ViewBag.activeManageQuiz = "active";
            ViewBag.listCourse = listCourse;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateQuiz(FormCollection form)
        {
            string quizId = form["quizId"].ToString();
            string question = form["question"].ToString();
            string answer = form["answer"].ToString();
            string option1 = form["option1"].ToString();
            string option2 = form["option2"].ToString();
            string option3 = form["option3"].ToString();
            string option4 = form["option4"].ToString();
            string category = form["category"].ToString();
            string sql = "UPDATE [dbo].[Quiz_Bank] SET [Question] = {0} ," +
                "[Option1] = {1} ,[Option2] = {2} ,[Option3] = {3} ,[Option4] = {4} ,[Answer] = {5} ,[CourseID] = {6} WHERE QuizID = {7}";
            db.Database.ExecuteSqlCommand(sql, question, option1, option2, option3, option4, answer, category, quizId);
            return RedirectToAction("ManageQuiz", "Admin", new { courseId = category });
        }
        public ActionResult DeleteQuiz()
        {

            string quizId = Request["quizId"].ToString();
            string courseId = Request["courseId"].ToString();
            db.Database.ExecuteSqlCommand("DELETE FROM dbo.Quiz_Bank WHERE QuizID = " + quizId);
            return RedirectToAction("ManageQuiz", "Admin", new { courseId = courseId });
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddQuiz(FormCollection form)
        {
            string question = form["question"].ToString().Trim();
            string answer = form["answer"].ToString().Trim();
            string option1 = form["option1"].ToString().Trim();
            string option2 = form["option2"].ToString().Trim();
            string option3 = form["option3"].ToString().Trim();
            string option4 = form["option4"].ToString().Trim();
            string category = form["category"].ToString().Trim();
            string sql = "INSERT INTO [dbo].[Quiz_Bank] ([Question] ,[Option1] ,[Option2] ,[Option3] ,[Option4] ,[Answer] ,[CourseID]) VALUES ({0} ,{1} ,{2} ,{3} ,{4} ,{5} ,{6})";
            db.Database.ExecuteSqlCommand(sql, question, option1, option2, option3, option4, answer, category);
            return RedirectToAction("ManageQuiz", "Admin", new { courseId = category });
        }
        public ActionResult ManageQuiz()
        {
            string courseId = "1";
            try
            {
                courseId = Request.QueryString["courseId"].ToString() == null ? "1" : Request["courseId"].ToString();
            }
            catch (Exception)
            {
                courseId = "1";
            }
            var ListCategory = db.Courses.SqlQuery("SELECT * FROM dbo.Course").ToList();
            var listQuiz = db.Quiz_Bank.SqlQuery("SELECT * FROM dbo.Quiz_Bank WHERE CourseID = " + courseId).ToList();
            ViewBag.listQuiz = listQuiz;
            ViewBag.activeManageQuiz = "active";
            ViewBag.courseId = courseId;
            ViewBag.ListCategory = ListCategory;
            return View();
        }
    }
}