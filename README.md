# Revit Ribbon Sample

This sample contains sample revit IExternalApplication, that contains creation of ribbon button in fluent style and moving it to the system tab

            panel = Ribbon.GetApplicationRibbon(application)
                .Tab("Мой tab")
                .Panel("Моя панелька", new CamelCaseToUnderscoreWithPrefixNameConvention(Resources.CompanyName))
                .CreateButton<Command>("Just test", button =>
                    button
                        .SetLargeImage(Resources.UndefinedIcon32)
                        .SetSmallImage(Resources.UndefinedIcon16));
            
            panel.MoveToSystemTab<Command>("Collaborate", "central_file_shr");

Project RevitUtils, that allows creation of ribon items in fluent style originated from Victor's VCRevitRibbonUtil https://github.com/chekalin-v/VCRevitRibbonUtil, but it contains some improvements.

Moving to system tab algorithm is based on Jeremy's blogpost: http://thebuildingcoder.typepad.com/blog/2014/07/moving-an-external-command-button-within-the-ribbon.html
