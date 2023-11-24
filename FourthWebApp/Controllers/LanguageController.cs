using Data_Access;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ViewModel;

namespace MvcMovie.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        MovieDalSql _movieDalSql = new MovieDalSql();

        [ActionName("Index")]
        public ActionResult AllLanguages()
        {
            
            return View();

        }
        public ActionResult GetLanguages([DataSourceRequest] DataSourceRequest request, string searchText)
        {
            List<MovieViewModel> language = _movieDalSql.GetAllLanguages(searchText);

            return Json(language.ToDataSourceResult(request));
        }

        public ActionResult ManageLanguage(int LanguageID = 0)
        {
            MovieViewModel model = new MovieViewModel();
            if (LanguageID != 0)
            {
                
                model = _movieDalSql.GetLanguageViewModel(LanguageID);
                
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageLanguage(MovieViewModel model)
        {
            try
            {
                //if (!_movieDalSql.IsLanguageNameUnique(model.LanguageID, model.LanguageName))
                //{
                    
                //    TempData["DuplicateCheck"] = true;
                    
                //}
                //else
                //{
                    if (model.LanguageID == 0)
                    {
                        _movieDalSql.AddLanguage(model.LanguageName);
                    }
                    else
                    {
                        _movieDalSql.UpdateLanguage(model);
                    }

                return JavaScript("CloseLanguage()");
                
            }

            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the language.");
                return View(model);

            }
        }

        public JsonResult CheckDupLanguage(int LanguageID, string LanguageName)
        {
            bool isExist = _movieDalSql.LanguageExist(LanguageID, LanguageName);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
    }
}