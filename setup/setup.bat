echo on

call azure account import %HOMEPATH%/Downloads/build.publishsettings

call git reset --hard
call git clean --force

cd ../client/html

cmd http-server