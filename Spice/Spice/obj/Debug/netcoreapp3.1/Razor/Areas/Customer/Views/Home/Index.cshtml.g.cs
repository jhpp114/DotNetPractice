#pragma checksum "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9314cb4fd740bcbd3bc4514445747fc246ac7df6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Customer_Views_Home_Index), @"mvc.1.0.view", @"/Areas/Customer/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\_ViewImports.cshtml"
using Spice;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\_ViewImports.cshtml"
using Spice.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9314cb4fd740bcbd3bc4514445747fc246ac7df6", @"/Areas/Customer/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6c1390ba8093fb4c2e21d25b459146d9962f6bcb", @"/Areas/Customer/Views/_ViewImports.cshtml")]
    public class Areas_Customer_Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Spice.Models.ViewModels.IndexViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("<br />\r\n<button class=\"btn btn-sm btn-primary displayCoup\">Display Available Coupons</button>\r\n<div class=\"border displayPossibleCoupon\" style=\"display:none\">\r\n");
#nullable restore
#line 8 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\Home\Index.cshtml"
     for (int i = 0; i < Model.Coupon.ToList().Count(); i++)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"carousel-item\">\r\n");
#nullable restore
#line 11 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\Home\Index.cshtml"
              
                string string64 = Convert.ToBase64String(Model.Coupon.ToList()[i].CouponImage);
                var image = string.Format($"data:image/jpg;base64,{string64}");
            

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n        <br />\r\n        <img");
            BeginWriteAttribute("src", " src=\"", 613, "\"", 625, 1);
#nullable restore
#line 18 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\Home\Index.cshtml"
WriteAttributeValue("", 619, image, 619, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" alt=\"Coupon Image\" width=\"80%\"/>\r\n");
#nullable restore
#line 19 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\Home\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n<br /><br />\r\n<main class=\"search_background_white\">\r\n");
#nullable restore
#line 23 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\Home\Index.cshtml"
     foreach(var menuItem in Model.MenuItem)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"row\">\r\n            <h3 class=\"text-success\">\r\n                ");
#nullable restore
#line 27 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\Home\Index.cshtml"
           Write(Html.DisplayFor(m => menuItem.Category.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </h3>\r\n        </div>\r\n");
            WriteLiteral("        <div class=\"border rounded border-info\" style=\"padding: 0.8rem;\">\r\n            <div class=\"row\">\r\n                <div class=\"col-md-3 col-sm-12\">\r\n                    <img");
            BeginWriteAttribute("src", " src=\"", 1129, "\"", 1150, 1);
#nullable restore
#line 34 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\Home\Index.cshtml"
WriteAttributeValue("", 1135, menuItem.Image, 1135, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" alt=\"Food Image\" width=\"90%\" style=\"border: 1px solid black; border-radius: 1rem;\" />\r\n                </div>\r\n                <div class=\"col-md-9 col-sm-12\">\r\n                    <p>");
#nullable restore
#line 37 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\Home\Index.cshtml"
                  Write(menuItem.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                </div>\r\n            </div>\r\n        </div>\r\n");
#nullable restore
#line 41 "C:\Users\jhpp1\Desktop\DotNetPractice\Spice\Spice\Areas\Customer\Views\Home\Index.cshtml"
        

    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</main>\r\n\r\n\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@" 
    <script>
        const displayCoup = document.querySelector('.displayCoup');
        displayCoup.addEventListener(""click"", toggleCoupon);
        
        function toggleCoupon() {
            const couponContainer = document.querySelector('.displayPossibleCoupon');
            console.log(couponContainer.getAttribute(""style""));
            if (couponContainer.getAttribute(""style"") == ""display:none"") {
                couponContainer.setAttribute(""style"", ""display:block"");
            } else {
                couponContainer.setAttribute(""style"", ""display:none"");
            }
        }
    </script>
");
            }
            );
            WriteLiteral(" ");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Spice.Models.ViewModels.IndexViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
