window.onload = init;

function init() {
    webSocket = new WebSocket("wss://localhost:7207/pouch");

    webSocket.onmessage = (event) => {
        const msg = JSON.parse(event.data);
    
        document.getElementById("coins").innerHTML = msg.PouchData.coins;
        document.getElementById("starpoints").innerHTML = msg.PouchData.star_points;
        document.getElementById("level").innerHTML = msg.PouchData.level;
        document.getElementById("basemaxhp").innerHTML = Math.max(0, (msg.PouchData.base_max_hp - 10) / 5);
        document.getElementById("basemaxfp").innerHTML = Math.max(0, (msg.PouchData.base_max_fp - 5) / 5);
        document.getElementById("totalbp").innerHTML = Math.max(0, (msg.PouchData.total_bp - 9) / 3);
        document.getElementById("jumplevel").src = "./resources/boots_" + msg.PouchData.jump_level + ".png";
        document.getElementById("hammerlevel").src = "./resources/hammer_" + msg.PouchData.hammer_level + ".png";

        update_party(document.getElementById("goombella"), msg.PouchData.party_data[1]);
        update_party(document.getElementById("koops"), msg.PouchData.party_data[2]);
        update_party(document.getElementById("flurrie"), msg.PouchData.party_data[5]);
        update_party(document.getElementById("yoshi"), msg.PouchData.party_data[4]);
        update_party(document.getElementById("vivian"), msg.PouchData.party_data[6]);
        update_party(document.getElementById("bobbery"), msg.PouchData.party_data[3]);
        update_party(document.getElementById("msmowz"), msg.PouchData.party_data[7]);

        // Update Yoshi color:
        let yoshi = document.getElementById("yoshi").querySelector(".party_member");
        let yoshi_color = (msg.PouchData.party_data[4].flags & 0xF000) >> 12;

        switch (yoshi_color) {
            case 0:
                yoshi.src = "./resources/yoshi_green.png";
                break;
            case 2:
                yoshi.src = "./resources/yoshi_red.png";
                break;
            case 4:
                yoshi.src = "./resources/yoshi_blue.png";
                break;
            case 6:
                yoshi.src = "./resources/yoshi_orange.png";
                break;
            case 8:
                yoshi.src = "./resources/yoshi_pink.png";
                break;
            case 10:
                yoshi.src = "./resources/yoshi_black.png";
                break;
            case 12:
                yoshi.src = "./resources/yoshi_white.png";
                break;
            default:
                yoshi.src = "./resources/yoshi_green.png";
                break;
        }

        update_star_power(document.getElementById("sweettreat"), msg.ModData.star_power_levels >> 0 & 3);
        update_star_power(document.getElementById("earthtremor"), msg.ModData.star_power_levels >> 2 & 3);
        update_star_power(document.getElementById("clockout"), msg.ModData.star_power_levels >> 4 & 3);
        update_star_power(document.getElementById("powerlift"), msg.ModData.star_power_levels >> 6 & 3);
        update_star_power(document.getElementById("artattack"), msg.ModData.star_power_levels >> 8 & 3);
        update_star_power(document.getElementById("sweetfeast"), msg.ModData.star_power_levels >> 10 & 3);
        update_star_power(document.getElementById("showstopper"), msg.ModData.star_power_levels >> 12 & 3);
        update_star_power(document.getElementById("supernova"), msg.ModData.star_power_levels >> 14 & 3);

        document.getElementById("floor").innerHTML = "Floor " + parseInt(msg.ModData.floor + 1, 10);
        document.getElementById("seed").innerHTML = msg.FileName ? msg.FileName : "Seed";
        document.getElementById("timer").innerHTML = msg.ModData.pit_start_time != 0 ? msg.PitRunElapsed : "00:00:00.00";

        
    }

    function update_party(party_member, party_data) {
        let party_img = party_member.querySelector(".party_member");
        let shine1 = party_member.querySelector('.shine1');
        let shine2 = party_member.querySelector('.shine2');
        let unlocked = party_data.flags & 1;

        party_img.style = unlocked ? "" : "filter: grayscale(1);";
        shine1.style = party_data.tech_level >= 1 ? "" : "display: none";
        shine2.style = party_data.tech_level >= 2 ? "" : "display: none";
    }

    function update_star_power(star_power, level) {
        let star_power_img = star_power.querySelector("img");
        let star_power_lvl = star_power.querySelector(".levelcount");

        if (level == 0) {
            star_power_img.style = "filter: grayscale(1);"
            star_power_lvl.style = "display: none;";
        }
        else {
            star_power_img.style = ""
            star_power_lvl.style = "";
            star_power_lvl.innerHTML = level;
        }
    }

    setInterval(function() {webSocket.send("ping")}, 1000)
}
