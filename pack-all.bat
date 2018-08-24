pushd NetCoreUtils
call pack-only.bat
popd

pushd NetCoreUtils.Database
call pack-with-dep-update.bat
popd