# Archived - 10/04/2021

---

# A nice way to offer Razor-Views Error Pages.#


### Steps

1. File -> New -> MVC Project.
2. Install-Package MagicalUnicorn.MvcErrorToolkit
3. Open up Global.asax or FilterConfig.cs, where the HandleErrorAttribute is getting added. Delete or comment this out.
4. Add your custom error files to web.config. Eg.

```<customErrors mode="On" defaultRedirect="~/views/error/Unknown.cshtml">
    <error statusCode="404" redirect="~/views/error/404.cshtml" />`
    <error statusCode="401" redirect="~/views/error/401.cshtml" />
    <error statusCode="500" redirect="~/views/error/500.cshtml" />
</customErrors>```

.5. Based on where you've defined your custom views, add the folder and the error view(s).

That's it. Enjoy!

*NOTE: This repository will get merged into my WorldDomination.Web.Mv repo, later*
