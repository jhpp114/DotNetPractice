#pragma checksum "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\Shared\MovieList.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a59c4eb5a15bebb243d4fac0a94db7c5c6f3562c"
// <auto-generated/>
#pragma warning disable 1591
namespace BlazorMovies.Client.Shared
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\_Imports.razor"
using BlazorMovies.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\_Imports.razor"
using BlazorMovies.Client.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\_Imports.razor"
using BlazorMovies.Client.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\_Imports.razor"
using BlazorMovies.Shared.Entities;

#line default
#line hidden
#nullable disable
    public partial class MovieList : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 1 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\Shared\MovieList.razor"
 if (Movies == null)
{

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(0, @"    <img src=""data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAATYAAACjCAMAAAA3vsLfAAAAz1BMVEX////6+vr89PT+/Pz87OL86Nr89vaxvIL4sXv8///l6NbM1c2istT4r3b2l3f9lamwwdft8OX+9/SYlcuwu9XV2uajtNLO1eS4xNfz8+z19fGxvIXM07utuXyyvIrEy6j98Ov66ur707n84c/f3sb3pmW2wZX5u4z3rob5wJb4q2/86OP5y6/71b398+z64uH219L3p4v1nYD6vK32kXH2tKH2oZP1zcP4s6r1w7T7sbj9i6L7nq7Y2tX4xsjh5eXh5+38v8mpq9Khnc61tNbzoKeKAAACnElEQVR4nO3Za1PaQBiG4d0EA4EECKBgkIq1Vm1toK2lrVCh6P//Td1NsuXUAn6KGe7LmRw2+bDzzLuHoBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgJyz7ax7kEe2ZZHbC5yexifHspz4gqrbR9jtnemzia1RKJDbbkGvGOdmKSJOrdDIuk85EL4pFnuBqTZbpUa17eO8WCyem9j6KrV+1j3KhUDF1rXTlZRi2+3i7aWO6F2v15XpCmpmtkZEeP9z5ftXdyqe8vsz0xT19XLaP1Iy7Njrdu2XSn5pc93UoR0VMuhQPlg3Orfb9eZCHBuDdIlcubOuP/j+jRW3S/PIjmstSU2uvn+w1mP4eHf7ab09KkRseFfpeGRaQ1K6riuEq+/1n36QPEpaFq8ffM3JZODpWFz3YjCwFvfpVXyyndBZff2wmTlMnYefq9Xql6+WaTe1JaQdlLWGMI0bY/vQpINPHaP7qnb/bdFuYktSK5eXUz5sS9U2SnL77qb3i9jCJLWAajMW4Qjn8sdo9LPvmrAWsckwCspBaEvmttTy0qgXUleYBXQpUHVQn/Zplqyk/+L0Nz4HpCMZllsNHsbjibPaJiuVyi/+HbPF47hWq03WPgl0bJXKNJse5cFQpzZ+XG+ekttWOrXaUF3M/o5TGapDGOeWXb9euYfa5Lcaoc68OTdNnXpb/9A7pdp2em42n9Reo9WSOrZ6J+v+5EO72dTVdux5x3FsdepsH3MVW1uIlue11CynYuuw+dht9qSKTQ3PE887UbOaLje2urvN1Mw2EyY2EarcZtn2KBfs1rPedIiO58WLQdhuZ9uhfGl7Hnm9nN1hKQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACA3PsD/R0hdMCZUboAAAAASUVORK5CYII="">
    ");
            __builder.AddMarkupContent(1, "<div>\r\n        <text>Loading...</text>\r\n    </div>\r\n");
#nullable restore
#line 7 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\Shared\MovieList.razor"
}
else if (Movies.Count() == 0)
{
    

#line default
#line hidden
#nullable disable
            __builder.AddContent(2, "There are no movies to show");
#nullable restore
#line 10 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\Shared\MovieList.razor"
                                            
}
else
{
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\Shared\MovieList.razor"
     foreach (var movie in Movies)
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(3, "        ");
            __builder.OpenElement(4, "p");
            __builder.AddContent(5, "Title: ");
            __builder.AddContent(6, 
#nullable restore
#line 16 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\Shared\MovieList.razor"
                   movie.Title

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(7, "\r\n        ");
            __builder.OpenElement(8, "p");
            __builder.AddContent(9, "Released Date: ");
            __builder.AddContent(10, 
#nullable restore
#line 17 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\Shared\MovieList.razor"
                           movie.ReleaseDate.ToString("dd/MMM/yyyy")

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(11, "\r\n");
#nullable restore
#line 18 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\Shared\MovieList.razor"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\Shared\MovieList.razor"
     
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 20 "C:\Users\jhpp1\Desktop\DotNetPractice\BlazorMovies\BlazorMovies\Client\Shared\MovieList.razor"
       
    [Parameter] public List<Movie> Movies { get; set; }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
