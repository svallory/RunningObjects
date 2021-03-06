using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace RunningObjects.MVC.Html
{
    public static class LinkExtensions
    {
        public static string WelcomeAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action(RunningObjectsAction.Welcome.ToString(), "Presentation", null);
        }

        public static string LogoutAction(this UrlHelper urlHelper)
        {
            var security = RunningObjectsSetup.Configuration.Security;
            if (security != null && security.IsAuthenticationConfigured)
            {
                var logout = security.Authentication<Object>().LogoutWith();
                var arguments = new { methodName = logout.Method.Name, redirectTo = urlHelper.WelcomeAction() };
                var values = CreateRouteValueDictionary(logout.Method.DeclaringType, arguments);
                return urlHelper.Action(RunningObjectsAction.Execute.ToString(), "Presentation", values);
            }
            return string.Empty;
        }

        public static string LoginAction(this UrlHelper urlHelper)
        {
            var security = RunningObjectsSetup.Configuration.Security;
            if (security != null && security.IsAuthenticationConfigured)
            {
                var loginWith = security.Authentication<Object>().LoginWith();
                var arguments = new { methodName = loginWith.Name, redirectTo = urlHelper.WelcomeAction() };
                var values = CreateRouteValueDictionary(loginWith.DeclaringType, arguments);
                return urlHelper.Action(RunningObjectsAction.Execute.ToString(), "Presentation", values);
            }
            return string.Empty;
        }

        public static string ApiAction(this UrlHelper urlHelper, Type modelType, RunningObjectsAction action, object arguments)
        {
            return urlHelper.Action(action.ToString(), "Service", CreateRouteValueDictionary(modelType, arguments));
        }

        public static string Action(this UrlHelper urlHelper, Type modelType, RunningObjectsAction action, object arguments)
        {
            return urlHelper.Action(action.ToString(), "Presentation", CreateRouteValueDictionary(modelType, arguments));
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, Type modelType, RunningObjectsAction action, object arguments = null)
        {
            return ActionLink(htmlHelper, linkText, modelType, action, arguments, null);
        }

        private static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, Type modelType, RunningObjectsAction action, object arguments, object htmlAttributes)
        {
            var routeValues = new RouteValueDictionary(htmlHelper.ViewContext.RouteData.Values);
            try
            {
                htmlHelper.ViewContext.RouteData.Values.Clear();
                return htmlHelper.ActionLink(linkText, action.ToString(), "Presentation",
                                             CreateRouteValueDictionary(modelType, arguments),
                                             HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }
            finally
            {
                foreach (var routeValue in routeValues)
                    htmlHelper.ViewContext.RouteData.Values.Add(routeValue.Key, routeValue.Value);
            }
        }

        private static RouteValueDictionary CreateRouteValueDictionary(Type modelType, object arguments)
        {
            return new RouteValueDictionary(arguments) { { "modelType", modelType.PartialName() } };
        }
    }
}
