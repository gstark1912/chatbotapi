require('dotenv').config();
const axios = require("axios");
const fs = require("fs");

const GITHUB_TOKEN = process.env.GITHUB_TOKEN;
const REPO = process.env.REPO;
const HOOK_ID = process.env.HOOK_ID;
const NGROK_API = "http://127.0.0.1:4040/api/tunnels";

async function main() {
  try {
    // Obtener URL actual de ngrok
    const tunnels = await axios.get(NGROK_API);
    const publicUrl = tunnels.data.tunnels.find(t => t.proto === "https").public_url;

    // Actualizar el webhook en GitHub
    await axios.patch(
      `https://api.github.com/repos/${REPO}/hooks/${HOOK_ID}`,
      {
        config: {
          url: `${publicUrl}/webhook`,
          content_type: "json"
        }
      },
      {
        headers: {
          Authorization: `token ${GITHUB_TOKEN}`,
          "User-Agent": "Webhook-Updater"
        }
      }
    );

    console.log(`✅ Webhook actualizado a: ${publicUrl}/webhook`);
  } catch (err) {
    console.error("❌ Error actualizando webhook:", err.message);
  }
}

main();
