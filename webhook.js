const http = require("http");
const { exec } = require("child_process");

const PORT = 3001;

http.createServer((req, res) => {
  if (req.method === "POST" && req.url === "/webhook") {
    let body = "";
    req.on("data", chunk => body += chunk);
    req.on("end", () => {
      console.log("🔁 Webhook received! Running deploy...");
      exec("powershell.exe -ExecutionPolicy Bypass -File ./deploy.ps1", (err, stdout, stderr) => {
        if (err) return console.error("❌", err);
        console.log(stdout);
      });
      res.writeHead(200);
      res.end("OK");
    });
  } else {
    res.writeHead(404);
    res.end();
  }
}).listen(PORT, () => {
  console.log(`🛰️ Webhook server listening on port ${PORT}`);
});