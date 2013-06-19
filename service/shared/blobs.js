var azure = require('azure');
var qs = require('querystring');
//console.log('a');
//var config = require('./config.js');
//console.log('b');
var util = require('util');

var config = {
	accountName : "azmob",
	accountKey : "jup/1UBhKhYCAZu7zyCO6okigetuCco7OGc9ea31kU+nMImVk3Dk7GKh9xt5GMVBdYW+VYAi9z06KvOIvJhnlQ==",
	imageContainer : "events"
};

var host = util.format('%s.blob.core.windows.net', config.storage.accountName);

function generateUrl(itemId) {
	var resource = util.format('%s/%s/%s', host, config.storage.imageContainer, itemId);

	var blobService = azure.createBlobService(config.storage.accountName, config.storage.accountKey, host);

	var policy = {
		AccessPolicy : {
			Permissions: azure.Constants.BlobConstants.SharedAccessPermissions.WRITE,
			Expiry: formatDate(new Date(new Date().getTime() + 5 * 60 * 1000))
		}
	}

	var sas = getSAS(config.storage.accountName, 
			config.storage.accountKey, 
			resource, 
			azure.Constants.BlobConstants.ResourceTypes.BLOB,
			policy);

	return util.format('http://%s?%s', resource, sas);
}

function getSAS(accountName, accountKey, path, resourceType, sharedAccessPolicy) {                         
     return qs.encode(new azure.SharedAccessSignature(accountName, accountKey)
                                    .generateSignedQueryString(path, {}, resourceType, sharedAccessPolicy));                           
}

function formatDate(date){ 
    var raw = date.toJSON();
    //blob service does not like milliseconds on the end of the time so strip
    return raw.substr(0, raw.lastIndexOf('.')) + 'Z'; 
}

exports.generateUrl = generateUrl;
