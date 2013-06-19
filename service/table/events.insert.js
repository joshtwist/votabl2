var _r = require;
require = function(path) {
	try {  return _r(path);	}
	catch (e) { return _r('../../app_data/config/scripts/table/' + path); }
}

var blobs = require('../shared/blobs.js');

function insert(item, user, request) {
    // todo - create an image URL for blob storage
    console.log(blobs);
    request.execute();
}