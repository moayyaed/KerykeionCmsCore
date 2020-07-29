using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace KerykeionCms.Pages
{
    public static class ManageSideNavPages
    {
        #region Props
        public static string WebPagesPage => "/WebPages";
        public static string WebPagePage => "/WebPage/Index";
        public static string WebPageArticleAddPage => "/WebPage/Articles/Add";
        public static string WebPageArticlePage => "/WebPage/Articles/Update";
        public static string WebPageArticlesPage => "/WebPage/Articles/Index";
        public static string ArticleAddPage => "/Articles/Add";
        public static string ArticlePage => "/Article/Index";
        public static string ArticlesPage => "/Articles/Index";
        public static string LinksPage => "/WebPage/Links";
        public static string LinkPage => "/WebPage/UpdateLink";
        public static string EntitiesPage => "/entities/index";
        public static string AddEntityPage => "/entities/add";
        public static string UpdateEntityPage => "/entities/update";
        public static string UsersPage => "/Users/Index";
        public static string AddUserPage => "/Users/Add";
        public static string RoleAddPage => "/Roleadd";
        public static string SubRolePage => "/Role";
        public static string ImagesPages => "/Images/Index";
        public static string ImageUpdatePage => "/Images/Update";
        #endregion


        #region WebpageFunx
        public static string WebPageAddNavActive(ViewContext viewContext, bool isDisable = false) => SetActive(viewContext, isDisable, WebPagesPage);
        public static string WebPageNavActive(ViewContext viewContext, bool isCurrentWebPage) => SetSubActive(viewContext, isCurrentWebPage, WebPagePage);
        public static string ArticlesNavActive(ViewContext viewContext, bool isCurrentWebPage) => SetSubActive(viewContext, isCurrentWebPage, WebPageArticlesPage, WebPageArticleAddPage);
        public static string ArticleNavActive(ViewContext viewContext, bool isCurrentArticle) => SetSubActive(viewContext, isCurrentArticle, WebPageArticlePage);
        public static string LinksNavActive(ViewContext viewContext, bool isCurrentWebPage) => SetSubActive(viewContext, isCurrentWebPage, LinksPage);
        public static string LinkNavActive(ViewContext viewContext, bool isCurrentLink) => SetSubActive(viewContext, isCurrentLink, LinkPage);
        public static string SubPagesDisplay(ViewContext viewContext) => DisplaySubs(viewContext, WebPagePage, WebPageArticleAddPage, WebPageArticlesPage, LinksPage, WebPageArticlePage, LinkPage);
        public static string SubWebPagesOpenCarretDisplay(ViewContext viewContext) => DisplayOpenCarret(viewContext, WebPagePage, WebPageArticleAddPage, WebPageArticlesPage, LinksPage, WebPageArticlePage, LinkPage);
        public static string SubWebPagesCloseCarretDisplay(ViewContext viewContext) => DisplayCloseCarret(viewContext, WebPagePage, WebPageArticleAddPage, WebPageArticlesPage, LinksPage, WebPageArticlePage, LinkPage);


        public static string SetWebPageSubTextClass(ViewContext viewContext, bool isCurrent) => SetTextClass(viewContext, isCurrent, WebPagePage);
        public static string SetArticleTextClass(ViewContext viewContext, bool isCurrent) => SetTextClass(viewContext, isCurrent, WebPageArticlePage);
        public static string SetArticlesTextClass(ViewContext viewContext, bool isCurrent) => SetTextClass(viewContext, isCurrent, WebPageArticleAddPage, WebPageArticlesPage);
        public static string SetLinkTextClass(ViewContext viewContext, bool isCurrent) => SetTextClass(viewContext, isCurrent, LinkPage);
        public static string SetLinksTextClass(ViewContext viewContext, bool isCurrent) => SetTextClass(viewContext, isCurrent, LinksPage);



        public static string PageSubOpenCarretDisplay(ViewContext viewContext, bool isCurrentPage) => DisplaySubOpenCarret(viewContext, isCurrentPage, WebPageArticleAddPage, WebPageArticlesPage, LinksPage, WebPageArticlePage, LinkPage);
        public static string PageSubCloseCarretDisplay(ViewContext viewContext, bool isCurrentPage) => DisplaySubCloseCarret(viewContext, isCurrentPage, WebPageArticleAddPage, WebPageArticlesPage, LinksPage, WebPageArticlePage, LinkPage);
        public static string PageArticlesOpenCarretDisplay(ViewContext viewContext, bool isCurrentPage) => DisplaySubOpenCarret(viewContext, isCurrentPage, WebPageArticlePage);
        public static string PageArticlesCloseCarretDisplay(ViewContext viewContext, bool isCurrentPage) => DisplaySubCloseCarret(viewContext, isCurrentPage, WebPageArticlePage);
        public static string PageLinksOpenCarretDisplay(ViewContext viewContext, bool isCurrentPage) => DisplaySubOpenCarret(viewContext, isCurrentPage, LinkPage);
        public static string PageLinksCloseCarretDisplay(ViewContext viewContext, bool isCurrentPage) => DisplaySubCloseCarret(viewContext, isCurrentPage, LinkPage);
        public static string PageSubsDisplay(ViewContext viewContext, bool isCurrentPage) => DisplaySubSubs(viewContext, isCurrentPage, WebPageArticleAddPage, WebPageArticlesPage, LinksPage, WebPageArticlePage, LinkPage);
        public static string PageArticlesDisplay(ViewContext viewContext, bool isCurrentPage) => DisplaySubSubs(viewContext, isCurrentPage, WebPageArticlePage);
        public static string PageLinksDisplay(ViewContext viewContext, bool isCurrentPage) => DisplaySubSubs(viewContext, isCurrentPage, LinkPage);
        public static string SubArticlesOpenFolderDisplay(ViewContext viewContext, bool isCurrentPage) => DisplayOpenFolder(viewContext, isCurrentPage, WebPageArticlePage);
        public static string WebpagesOpenFolderDisplay(ViewContext viewContext) => DisplayOpenFolder(viewContext, WebPageArticleAddPage, WebPageArticlesPage, LinksPage, WebPageArticlePage, LinkPage, WebPagePage);
        public static string LinksOpenFolderDisplay(ViewContext viewContext, bool isCurrentPage) => DisplayOpenFolder(viewContext, isCurrentPage, LinkPage);

        #endregion

        #region BaseArticlesFunx
        public static string ArticleAddNavActive(ViewContext viewContext) => SetSubActive(viewContext, true, ArticlesPage, ArticleAddPage);
        public static string ArticlesOpenCarretDisplay(ViewContext viewContext) => DisplayOpenCarret(viewContext, ArticlePage);
        public static string ArticlesCloseCarretDisplay(ViewContext viewContext) => DisplayCloseCarret(viewContext, ArticlePage);
        public static string BaseArticleNavActive(ViewContext viewContext, bool isCurrentArticle) => SetSubActive(viewContext, isCurrentArticle, ArticlePage);
        public static string SubArticlesDisplay(ViewContext viewContext) => DisplaySubs(viewContext, ArticlePage);
        public static string SetBaseArticleTextClass(ViewContext viewContext, bool isCurrent) => SetTextClass(viewContext, isCurrent, ArticlePage);
        public static string SetBaseArticlesTextClass(ViewContext viewContext, bool isCurrent) => SetTextClass(viewContext, isCurrent, ArticleAddPage, ArticlesPage);
        public static string ArticlesOpenFolderDisplay(ViewContext viewContext) => DisplayOpenFolder(viewContext, ArticlePage);

        #endregion

        #region EntityFunx
        public static string TableNavActive(ViewContext viewContext, bool isCurrentTable) => SetSubActive(viewContext, isCurrentTable, EntitiesPage, AddEntityPage);
        public static string EntityNavActive(ViewContext viewContext, bool isCurrentTable) => SetSubActive(viewContext, isCurrentTable, UpdateEntityPage, AddEntityPage);
        public static string TableSubOpenCarretDisplay(ViewContext viewContext, bool isCurrentTable) => DisplaySubOpenCarret(viewContext, isCurrentTable, UpdateEntityPage);
        public static string TableSubCloseCarretDisplay(ViewContext viewContext, bool isCurrentTable) => DisplaySubCloseCarret(viewContext, isCurrentTable, UpdateEntityPage);
        public static string SetTableTextClass(ViewContext viewContext, bool isCurrent) => SetTextClass(viewContext, isCurrent, EntitiesPage, AddEntityPage);
        public static string SetEntityTextClass(ViewContext viewContext, bool isCurrent) => SetTextClass(viewContext, isCurrent, UpdateEntityPage);
        public static string TableEntitiesDisplay(ViewContext viewContext, bool isCurrentTable) => DisplaySubSubs(viewContext, isCurrentTable, UpdateEntityPage);
        public static string SubEntitiesDisplay(ViewContext viewContext) => DisplaySubs(viewContext, EntitiesPage, UpdateEntityPage, AddEntityPage);
        public static string SubEntitiesOpenCarretDisplay(ViewContext viewContext) => DisplayOpenCarret(viewContext, EntitiesPage, UpdateEntityPage, AddEntityPage);
        public static string SubEntitiesOpenFolderDisplay(ViewContext viewContext) => DisplayOpenFolder(viewContext, EntitiesPage, UpdateEntityPage, AddEntityPage);
        public static string SubEntitiesSubOpenFolderDisplay(ViewContext viewContext, bool isCurrentEntity) => DisplayOpenFolder(viewContext, isCurrentEntity, UpdateEntityPage, AddEntityPage);
        public static string SubEntitiesCloseCarretDisplay(ViewContext viewContext) => DisplayCloseCarret(viewContext, EntitiesPage, UpdateEntityPage, AddEntityPage);
        #endregion

        #region UserFunx
        public static string UsersNavActive(ViewContext viewContext, bool isDisable = false) => SetActive(viewContext, isDisable, UsersPage, AddUserPage);
        #endregion

        #region RolesFunx
        public static string RoleAddNavActive(ViewContext viewContext, bool isDisable = false) => SetActive(viewContext, isDisable, RoleAddPage);
        public static string SubRolesDisplay(ViewContext viewContext) => DisplaySubs(viewContext, SubRolePage);
        public static string SubRolesOpenCarretDisplay(ViewContext viewContext) => DisplayOpenCarret(viewContext, SubRolePage);
        public static string SubRolesCloseCarretDisplay(ViewContext viewContext) => DisplayCloseCarret(viewContext, SubRolePage);
        public static string SubRolesOpenFolderDisplay(ViewContext viewContext) => DisplayOpenFolder(viewContext, SubRolePage);
        #endregion

        #region ImagesFunx
        public static string SubImagesOpenFolderDisplay(ViewContext viewContext) => DisplayOpenFolder(viewContext, ImageUpdatePage);
        public static string ImagesNavActive(ViewContext viewContext, bool isDisable = false) => SetActive(viewContext, isDisable, ImagesPages);
        public static string SubImagesDisplay(ViewContext viewContext) => DisplaySubs(viewContext, ImageUpdatePage);
        public static string SubImagesOpenCarretDisplay(ViewContext viewContext) => DisplayOpenCarret(viewContext, ImageUpdatePage);
        public static string SubImagesCloseCarretDisplay(ViewContext viewContext) => DisplayCloseCarret(viewContext, ImageUpdatePage);
        #endregion


        private static string DisplaySubOpenCarret(ViewContext viewContext, bool isCurrentEntity, params string[] pages)
        {
            return (IsOnAnyOfTheSpecifiedPages(viewContext, pages) && isCurrentEntity) ? "d-none" : "d-inline-block";
        }
        private static string DisplaySubCloseCarret(ViewContext viewContext, bool isCurrentEntity, params string[] pages)
        {
            return (IsOnAnyOfTheSpecifiedPages(viewContext, pages) && isCurrentEntity) ? "d-inline-block" : "d-none";
        }
        private static string DisplaySubSubs(ViewContext viewContext, bool isCurrentEntity, params string[] pages)
        {
            return (IsOnAnyOfTheSpecifiedPages(viewContext, pages) && isCurrentEntity) ? "" : "d-none";
        }

        private static string SetTextClass(ViewContext viewContext, bool isCurrent, params string[] pages)
        {
            return (IsOnAnyOfTheSpecifiedPages(viewContext, pages) && isCurrent) ? "disabled text-dark" : "";
        }

        private static string SetActive(ViewContext viewContext, bool isDisable, params string[] pages)
        {
            if (!isDisable)
            {
                return IsOnAnyOfTheSpecifiedPages(viewContext, pages) ? "bg-secondary" : "";
            }
            return IsOnAnyOfTheSpecifiedPages(viewContext, pages) ? "disabled text-dark" : "";
        }

        private static string SetSubActive(ViewContext viewContext, bool isCurrentEntity, params string[] pages)
        {
            return (IsOnAnyOfTheSpecifiedPages(viewContext, pages) && isCurrentEntity) ? "disabled bg-secondary" : "";
        }

        private static string DisplaySubs(ViewContext viewContext, params string[] pages)
        {
            return IsOnAnyOfTheSpecifiedPages(viewContext, pages) ? "" : "d-none";
        }

        private static string DisplayOpenCarret(ViewContext viewContext, params string[] pages)
        {
            return IsOnAnyOfTheSpecifiedPages(viewContext, pages) ? "d-none" : "d-inline-block";
        }

        private static string DisplayOpenFolder(ViewContext viewContext, bool isCurrent, params string[] pages)
        {
            return IsOnAnyOfTheSpecifiedPages(viewContext, pages) && isCurrent ? "fa-folder-open-o" : "fa-folder-o";
        }
        private static string DisplayOpenFolder(ViewContext viewContext, params string[] pages)
        {
            return IsOnAnyOfTheSpecifiedPages(viewContext, pages) ? "fa-folder-open-o" : "fa-folder-o";
        }

        private static string DisplayCloseCarret(ViewContext viewContext, params string[] pages)
        {
            return IsOnAnyOfTheSpecifiedPages(viewContext, pages) ? "d-inline-block" : "d-none";
        }

        private static string GetCurrent(ViewContext viewContext)
        {
            return viewContext.RouteData.Values["Page"].ToString();
        }

        private static bool IsOnAnyOfTheSpecifiedPages(ViewContext viewContext, params string[] pages)
        {
            var current = GetCurrent(viewContext);
            foreach (var page in pages)
            {
                if (string.Equals(current, page, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
