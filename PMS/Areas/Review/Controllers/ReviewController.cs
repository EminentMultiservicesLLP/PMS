using PMS.Filters;
using System.Web.Mvc;

namespace PMS.Areas.Review.Controllers
{
    [CheckSession]
    public class ReviewController : Controller
    {
        //
        // GET: /Review/Review/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FirstReview()
        {
            return PartialView();
        }

        public ActionResult SecondReview ()
        {
            return PartialView();
        }

        public ActionResult ReviewReports()
        {
            return PartialView();
        }
    }
}
