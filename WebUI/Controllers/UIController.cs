﻿using Microsoft.AspNetCore.Mvc;
using WebUI.Utils.Extensions;

namespace WebUI.Controllers;

public class UIController : Controller
{
    public IActionResult SetLightMode(string mode, string? returnUrl)
    {
        HttpContext.SetLightMode(mode);

        if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

        var uri = new Uri(returnUrl);
        if (uri.Host != Request.Host.Host) return RedirectToAction("Index", "Home");

        if (Request.Path.HasValue && (uri.LocalPath == Request.Path.Value)) return RedirectToAction("Index", "Home");

        return Redirect(returnUrl);
    }
}
