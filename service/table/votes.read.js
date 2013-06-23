// This script simply returns a signed, temporary token that allows you to insert votes.
// this technique was added for the live demo, to simply _slow_ up anybody who tried to falsely
// flood the system with votes using a tool like fiddler. It is completely fallible but hopefully
// buys enough time to get through the live streaming demo OK. To correctly implement throttling, 
// you'd have users authenticate - but that would have slowed things up too much! Have fun :)

var key = "GgIoe4FneNtKK+WkKQq+ropj7mB98XYUNZnt5fV+7V0=";

function read(query, user, request) {
	var expiry = new Date();
	expiry.setSeconds(expiry.getSeconds() + 7); // insert is valid 7 seconds from generation
	var x = expiry.getTime().toString();
	var token = x + "." + signature(x);
	request.respond(200, [{ token: x}]);
}

function signature(input) {
	var key = crypto.createHash('sha256').update(key).digest('binary');
	var str = crypto.createHmac('sha256', key).update(input).digest('base64');
	return str;
}