exports.post = function(request, response) {
    // Use "request.service" to access features of your mobile service, e.g.:
    //   var tables = request.service.tables;
    //   var push = request.service.push;
    console.log(request.body);
    console.log(request.data);
    response.send(200, [{ a : 1}, { b : 2} ]);
};