using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        Project_CSharpEntities1 db = new Project_CSharpEntities1();
        // GET: Home
        public ActionResult Index()
        {
            //var listCourses = db.Courses;
            loadNavBar();
            return View();
        }
        public void loadNavBar()
        {
            var listCourses = db.Courses.SqlQuery("SELECT * FROM dbo.Course");
            ViewBag.listC = listCourses;
        }
        public ActionResult Learn()
        {
            string courseId = Request.QueryString["courseId"];
            string page = Request.QueryString["page"] == null ? "1" : Request.QueryString["page"].ToString();
            var listCourses = db.Courses;
            int cId = Convert.ToInt32(courseId);
            int totalPage = db.Course_Lesson.Where(o => o.CourseID == cId).Count();
            int endPage = totalPage / 3 + (totalPage % 3 == 0 ? 0 : 1);
            //var listLesson = db.Course_Lesson.Where(n => n.CourseID == cId);
            int offset = (Convert.ToInt32(page) - 1) * 3;
            var listLesson = db.Course_Lesson.SqlQuery("SELECT * FROM dbo.Course_Lesson WHERE CourseID = " + courseId + " ORDER BY LessonID OFFSET " + offset + " ROWS FETCH NEXT 3 ROWS ONLY");
            string courseName = db.Courses.SqlQuery("SELECT * FROM dbo.Course WHERE CourseID = " + courseId).FirstOrDefault().CourseName;
            ViewBag.courseName = courseName;
            ViewBag.page = Convert.ToInt32(page);
            ViewBag.cid = cId;
            ViewBag.listC = listCourses;
            ViewBag.endPage = endPage;
            ViewBag.listL = listLesson;
            return View();
        }
        public ActionResult NextLesson()
        {
            loadNavBar();
            string lessonId = Request.QueryString["lessonId"];
            string courseId = Request.QueryString["courseId"];
            string lastCourseId = db.Course_Lesson.SqlQuery("SELECT * FROM dbo.Course_Lesson " +
                "WHERE CourseID = " + courseId + " ORDER BY LessonID DESC").Max(o => o.LessonID).ToString();
            string firstCourseId = db.Course_Lesson.SqlQuery("SELECT * FROM dbo.Course_Lesson " +
                "WHERE CourseID = " + courseId + " ORDER BY LessonID DESC").Min(o => o.LessonID).ToString();
            Course_Lesson course_Lesson = db.Course_Lesson.SqlQuery("SELECT TOP 1 * FROM dbo.Course_Lesson WHERE LessonID > " + lessonId + " AND CourseID = " + courseId + " ORDER BY LessonID").FirstOrDefault();
            ViewBag.cid = Convert.ToInt32(courseId);
            ViewBag.firstCourseId = firstCourseId;
            ViewBag.lastCourseId = lastCourseId;
            ViewBag.Lesson = course_Lesson;
            return View("LessonDetail");
        }
        public ActionResult PreviousLesson()
        {
            loadNavBar();
            string lessonId = Request.QueryString["lessonId"];
            string courseId = Request.QueryString["courseId"];
            string lastCourseId = db.Course_Lesson.SqlQuery("SELECT * FROM dbo.Course_Lesson " +
                "WHERE CourseID = " + courseId + " ORDER BY LessonID DESC").Max(o => o.LessonID).ToString();
            string firstCourseId = db.Course_Lesson.SqlQuery("SELECT * FROM dbo.Course_Lesson " +
                "WHERE CourseID = " + courseId + " ORDER BY LessonID DESC").Min(o => o.LessonID).ToString();
            Course_Lesson course_Lesson = db.Course_Lesson.SqlQuery("SELECT TOP 1 * FROM dbo.Course_Lesson WHERE LessonID < " + lessonId + " AND CourseID = " + courseId + " ORDER BY LessonID DESC").FirstOrDefault();
            ViewBag.cid = Convert.ToInt32(courseId);
            ViewBag.firstCourseId = firstCourseId;
            ViewBag.lastCourseId = lastCourseId;
            ViewBag.Lesson = course_Lesson;
            return View("LessonDetail");
        }
        public ActionResult LessonDetail()
        {
            loadNavBar();
            string lessonId = Request.QueryString["lessonId"];
            string courseId = Request.QueryString["courseId"];
            string lastCourseId = db.Course_Lesson.SqlQuery("SELECT * FROM dbo.Course_Lesson " +
                "WHERE CourseID = " + courseId + " ORDER BY LessonID DESC").Max(o => o.LessonID).ToString();
            string firstCourseId = db.Course_Lesson.SqlQuery("SELECT * FROM dbo.Course_Lesson " +
                "WHERE CourseID = " + courseId + " ORDER BY LessonID DESC").Min(o => o.LessonID).ToString();
            Course_Lesson course_Lesson = db.Course_Lesson.SqlQuery("SELECT * FROM dbo.Course_Lesson " +
                "WHERE CourseID = " + courseId + " AND LessonID = " + lessonId).FirstOrDefault();
            ViewBag.cid = Convert.ToInt32(courseId);
            ViewBag.firstCourseId = firstCourseId;
            ViewBag.lastCourseId = lastCourseId;
            ViewBag.Lesson = course_Lesson;
            return View();
        }
        public ActionResult QuizTest()
        {
            loadNavBar();
            string courseId = Request.QueryString["courseId"];
            string courseName = db.Courses.SqlQuery("SELECT * FROM dbo.Course WHERE CourseID = " + courseId).FirstOrDefault().CourseName;
            int cId = Convert.ToInt32(courseId);
            int totalQuestion = db.Quiz_Bank.Where(o => o.CourseID == cId).Count();
            Quiz_Bank quiz = db.Quiz_Bank.SqlQuery("SELECT * FROM dbo.Quiz_Bank WHERE CourseID = " + courseId + " ORDER BY (SELECT NULL) " +
                "OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY").FirstOrDefault();
            ViewBag.totalTrueAnswer = 0;
            ViewBag.cId = cId;
            ViewBag.currentQuestion = 1;
            ViewBag.quiz = quiz;
            ViewBag.courseName = courseName;
            ViewBag.totalQuestion = totalQuestion;
            return View();
        }
        [HttpPost,ValidateInput(false)]
        public ActionResult QuizTest(FormCollection f)
        {
            loadNavBar();
            string courseId = Request.QueryString["courseId"];
            string courseName = db.Courses.SqlQuery("SELECT * FROM dbo.Course WHERE CourseID = " + courseId).FirstOrDefault().CourseName;
            int cId = Convert.ToInt32(courseId);
            int totalQuestion = db.Quiz_Bank.Where(o => o.CourseID == cId).Count();
            string currentQuestion = f["currentQuestion"].ToString();
            Quiz_Bank quiz = db.Quiz_Bank.SqlQuery("SELECT * FROM dbo.Quiz_Bank WHERE CourseID = " + courseId + " ORDER BY (SELECT NULL) " +
           "OFFSET " + currentQuestion + " ROWS FETCH NEXT 1 ROWS ONLY").FirstOrDefault();
            string quizId = f["quizId"].ToString();
            string answer = f.Get("quiz").ToString() == null ? "notanswer" : f.Get("quiz").ToString();
            int qId = Convert.ToInt32(quizId);
            bool isTrueAnswer = db.Quiz_Bank.Where(o => (o.QuizID == qId) && (o.Answer.Equals(answer))).Count() == 1;
            int totalTrueAnswer = isTrueAnswer ? Convert.ToInt32(f["totalTrueAnswer"].ToString()) + 1 : Convert.ToInt32(f["totalTrueAnswer"].ToString());
            double percent = (double)((totalTrueAnswer * 100) / totalQuestion);
            string percentScore = String.Format("{0:0.##}", percent);
            db.Database.ExecuteSqlCommand("UPDATE dbo.Quiz_Bank SET UserAnswer = '" + answer + "' WHERE QuizID = " + quizId);
            ViewBag.percent = percentScore;
            ViewBag.isTrueAnswer = isTrueAnswer;
            ViewBag.totalTrueAnswer = totalTrueAnswer;
            ViewBag.currentQuestion = Convert.ToInt32(currentQuestion) + 1;
            ViewBag.cId = cId;
            ViewBag.quiz = quiz;
            ViewBag.courseName = courseName;
            ViewBag.totalQuestion = totalQuestion;
            return View();
        }

        public ActionResult QuizResults()
        {
            loadNavBar();
            string courseId = Request.QueryString["courseId"];
            var quiz_Banks = db.Quiz_Bank.SqlQuery("SELECT * FROM dbo.Quiz_Bank WHERE CourseID = " + courseId);
            string courseName = db.Courses.SqlQuery("SELECT * FROM dbo.Course WHERE CourseID = " + courseId).FirstOrDefault().CourseName.ToString();
            ViewBag.cid = Convert.ToInt32(courseId);
            ViewBag.courseName = courseName;
            ViewBag.quiz_Banks = quiz_Banks;
            return View();
        }
    }
}