function insert(item, user, request) {
																																checkToken(item, request);
	request.execute({
		success: function() {
			mssql.query('SELECT DISTINCT channelUri FROM channels', [], {
				success : function(results) {
					request.respond();
					results.forEach(function(r) {
						push.wns.sendRaw(r.channelUri, JSON.stringify(item));
					});
				}
			});
		}
	});
}
















var crypto = require('crypto');

var signatureKey = "GgIoe4FneNtKK+WkKQq+ropj7mB98XYUNZnt5fV+7V0=";

function checkToken(item, request) {
	var parts = item.token.split('.');
	var date = new Date(parseInt(parts[0]));
	var sig = parts[1];
	if (new Date() > date) {
		request.respond(400, "Your voting token expired, try again");
		throw new Error('Token expired ' + item.token);
	}
	var validationSignature = signature(parts[0])
	var valid = validationSignature == parts[1];
	if (!valid) {
		request.respond(400, "No valid voting token");
		throw new Error('Invalid Signature ' + item.token + " - " + validationSignature);
	}

	delete item.token;
}

function signature(input) {
	var key = crypto.createHash('sha256').update(signatureKey).digest('binary');
	var str = crypto.createHmac('sha256', key).update(input).digest('base64');
	return str;
}