using ODSphereRouter.Models;
using System.ComponentModel.DataAnnotations;

namespace ODSphereRouter.DTOs
{
    /* Default data for sphere database
     * migrationBuilder.InsertData(table: "Spheres",
                columns: new[] { nameof(SphereDTO.Name), nameof(SphereDTO.Systems) },
                values: new object[,] 
                {
                    { "Aowica",                   string.Join(",", "Aowica", "Callites", "Cians", "Coquiabayo", "Heidumani", "HIP 79682", "HIP 81390", "HIP 83217", "Hirpasuk", "Hun Dun", "Hunzicnal", "Kusang", "Mo Daorsemi", "Shatrughna", "Trumbo", "Unkunder", "Windumlia") },
                    { "Aurawala",                 string.Join(",", "Aurawala","Bediangu","Boloine","Byerri","Caduwar","Carns","Dyinyinga","HIP 110397","HIP 110867","HIP 12921","HIP 14759","HIP 32897","HIP 3470","HIP 3911","HIP 9191","Ki Lao Kana","Kundjiban","Lai Shui","Lexovit","Sambuwa","Shabos","Su Lisungu","Zorynian") },
                    { "Ataokomo",                 string.Join(",", "Ataokomo","Bela Oh","Bhadi","Cartoi","Goeng Gong","HIP 27633","Hun Pef","Jaroua","Kamunkula","Kokonda","Kotandji","Kurna","Lieldi","Liu Beserka","Oloku","Ronggi","Silvait","Tjeraringu","Visthalam") },
                    { "Chankates",                string.Join(",", "Adlivuni","Assiones","Cashikori","Chankates","Col 285 Sector QE-M b22-3","HIP 76713","HIP 77818","HIP 79315","HIP 79735","HIP 81353","Khan Gubii","Li Nang Yi","Paesia","Tehuevisc","Vash","Wu Jinifex","Xpiyacoc") },
                    { "Ekono",                    string.Join(",", "Aiga", "Arimavante","Cocovicente","Cowini","Ekono","Fengol","Hannheir","HIP 114880","Kanuket","Rianindana","San Guerets","Snotock Ek","TaexaliTemu","Wangai")},
                    { "Gally Bese",               string.Join(",", "Belata","Bhatas","Damontae","Freyan","Gally Bese","Gero Kiaku","HIP 33390","HIP 34698","HIP 35246","HIP 36352","HIP 37918","Kolandira","Moroni","Venexo","Vidu")},
                    { "Haroingori",               string.Join(",", "Awang","Callici","Damoch","Ekoimunggu","Gunno","Haolin Ti","Haroingori","HIP 77310","HIP 82820","Iota Trianguli Australis","Kamaki","Kulkan Lung","LTT 6299","Lu Pinda","Mac Og","Niaba","Nik","Peng","Pisamazin","Ruwachis","Sesui Redos","Shake","Shanhauls","Shen Yi","Siaroju","Sitakapa","Turona","Wongi","Xelardi","Yakaeriate") },
                    { "HIP 116045",               string.Join(",", "Chukuru","Duroua","Galonto","HIP 114040","HIP 114614","HIP 114967","HIP 115275","HIP 115386","HIP 115917","HIP 116045","HIP 116377","HIP 116554","HIP 116890","HIP 117187","Ikpe","Kaupolo","Shattvasu") },
                    { "Jotunheim",                string.Join(",", "Atlantis","Beta-1 Tucanae","Caspatsuria","CD-67 2611","Cegreeth","CT Tucanae","Daikulcandi","Fotla","Futhark","Galouri","Gondul","Grovichun","Jotunheim","LTT 9810","Ngadjal","Ngurungo","Rho Tucanae","Saffron","Sigurd","Wanjan","CD-69 5","CD-70 1960")},
                    { "Kapoongzi",                string.Join(",", "Arungu","Basikasingo","Coana","Durgeth","Er Long","Garoju","Gelo Kop","HIP 104453","HIP 106442","HR 8380","Kapoongzi","Kimih","Mbukas","Njanheim","Ogungurugh","Okundu","Segnir","Shebkauluwa","Tekkeenat","Wanjul") },
                    { "Mahiko",                   string.Join(",", "Acokwech","Ailurui","Albarib","Atrimih","Banapityas","Bean Nighe","CD-51 4783","Chandh","Choenpetese","Dountidi","GCRV 60796","LHS 2150","Lokit","Mahiko","Mangwe","Milceni","No Mina","Nunet","Pilintana","Poqomawi","Quariti","Regnenses","Riki","SAO 221526","Sengen Sama","Sirsir","Sounti","Wepaw") },
                    { "Mangiliri",                string.Join(",", "Malgariji","Taiocamu","HIP 35185","HIP 33275","Taurinja","Tchehen","Ailuremowo","Hyades Sector TO-P b6-3","Gerong","Eravians","Zememede","Eventiensi","Mangiliri","Bach","Xbaquitae","Ross 609")},
                    { "Perktomen",                string.Join(",", "Ainun","Antliae Sector CQ-Y b3","Antliae Sector DL-Y c4","Giguen","Gliese 9318","HIP 49599","HIP 49767","HIP 50032","HIP 50805","HIP 50807","HIP 52091","Hranyi","Kwanteru","Nebther","Perktomen","Tibila","Tricastini","Tylis","Zuruaha") },
                    { "Reieni",                   string.Join(",", "Aobrigen Nu","Baadaga","Fugen Nones","Goibniugo","Gommatchal","HIP 50197","HIP 50765","HIP 52552","HIP 54405","HIP 54846","Karamai","Knumclaw","Mantunt","Mbamunians","Nagnutli","Nanatani","Omukate","Perce","Reieni","Rudrato","Rukminiez","Tujil","Yu Song Di") },
                    { "Runapa",                   string.Join(",", "Arthaiyati","Badjilasta","Bylgiani","Djinajeri","Gliese 9468","Gucuman","HIP 68162","HR 5349","Karamo","Kunaheo","Kungadutji","Othe","Runapa","Shakal","Usdian") },
                    { "Simyr",                    string.Join(",", "Babai","Hinus","HIP 33109","Iah","Minung Po","Murni","Niamara","Ogunda","Prahasi","San Huang","Sarana","Segeirre","Simyr","Surti","Tatji","Tharviri","Triviatii","Wakam","Wirdisci","Yamatji","Zhu Vedani","Zosinambure") },
                    { "Syntheng",                 string.Join(",", "Goplang","HIP 107994","HIP 108701","HIP 108729","HIP 108971","HIP 109787","Honoss","Kosretia","Mantoac","Nemetati","Sokomi","Syntheng","Taexali","Yaguvit") },
                    { "Ts'ai Shai",               string.Join(",", "Amenhit","Bedaramojas","Benie","Cavane","Coelerni","HIP 100449","HIP 101549","Imhottii","Lahau","Li Nones","Mossarian","Niu Anang","Pichi Wang","Ts'ai Shai","Woyemsit","Zemete","Zlotaikua") },
                    { "Yan Musu",                 string.Join(",", "Argel","Ariku","Chert","Chiutl","Cupiaci","Ekondhri","Ginnae","HIP 83247","HIP 85297","HIP 87638","Ix Chuntin","Khongyan","Lidar","Mugari","Yan Musu") },
                    { "Zeta Trianguli Australis", string.Join(",", "Baker","Beta Trianguli Australis","Daiku","Hill Pa Hsi","Iman Caber","Jiuyou","L 206-187","LFT 1300","LFT 1349","LHS 3167","LHS 3295","LTT 7509","Mari","Moepirt","Mombaluma","Nemehi","Santy","Scori","Zeaex","Zeta Trianguli Australis")}
                }
            );*/
    public sealed class SphereDTO
    {
        [Key]
        public string Name { get; set; }

        public string Systems { get; set; }

        public SphereDTO(string name, string systems)
        {
            Name = name;
            Systems = systems;
        }

        public SphereDTO(Sphere sphere)
        {
            Name = sphere.ControllingSystem;
            Systems = string.Join(",", sphere.Systems);
        }
    }
}
