Edmgen.exe /provider:EFOracleProvider /mode:fullgeneration /connectionstring:"data source=smtsaas;user id=smtfb; password=smtfb" /project:SMT_FB_EFModel
ReplaceString.exe SMT_FB_EFModel.ssdl
Edmgen2.exe /toedmx SMT_FB_EFModel.csdl SMT_FB_EFModel.msl SMT_FB_EFModel.ssdl
pause