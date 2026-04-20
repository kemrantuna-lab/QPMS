;#DropDB QPMSEasyTest

#Application QPMSWin
;#Application QPMSWeb

*FillForm
 User Name = Admin
*Action Log In

#IfDef QPMSWin

*Action About...
 
*Action OK

#EndIf

#IfDef QPMSWeb
*Action Navigation(My Details)
#EndIf
