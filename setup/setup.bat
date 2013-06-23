echo on

call azure account import %HOMEPATH%/Downloads/build.publishsettings

cd ../

call git reset --hard
call git clean --force

cd client/html

call http-server