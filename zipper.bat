del "C:\Proyectos VB.Net\PentaUpdate\*.zip" /s /q
xcopy /s "C:\Proyectos VB.Net\PentaUpdate\Output" "C:\Proyectos VB.Net\PentaUpdate\Output2\"
del "C:\Proyectos VB.Net\PentaUpdate\Output2\*.config" /s /q
del "C:\Proyectos VB.Net\PentaUpdate\Output2\*.application" /s /q
del "C:\Proyectos VB.Net\PentaUpdate\Output2\*.manifest" /s /q
del "C:\Proyectos VB.Net\PentaUpdate\Output2\*.pdb" /s /q
del "C:\Proyectos VB.Net\PentaUpdate\Output2\*.ini" /s /q
del "C:\Proyectos VB.Net\PentaUpdate\Output2\*.xml" /s /q
del "C:\Proyectos VB.Net\PentaUpdate\Output2\*.lnk" /s /q
@RD /S /Q "C:\Proyectos VB.Net\PentaUpdate\Output2\Logs"
@RD /S /Q "C:\Proyectos VB.Net\PentaUpdate\Output2\app.publish"
"C:\Program Files\7-Zip\7z" a -tzip "C:\Proyectos VB.Net\PentaUpdate\PentaUpdate.zip" "C:\Proyectos VB.Net\PentaUpdate\Output2\*.*" -mx5
"C:\Program Files\7-Zip\7z" x "C:\Proyectos VB.Net\PentaUpdate\PentaUpdate.zip" -o"C:\Proyectos VB.Net\PentaUpdate\PentaUpdate" -aoa
@RD /S /Q "C:\Proyectos VB.Net\PentaUpdate\Output2"
echo File PentaUpdate.zip / Cartella PentaUpdate creati
start %windir%\explorer.exe "C:\Proyectos VB.Net\PentaUpdate\PentaUpdate" 
pause