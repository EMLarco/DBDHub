-- ============================================================
-- DBD Hub - Datos de catalogo (Killers, Survivors, Maps, Addons)
-- Fuente: Dead by Daylight (Behaviour Interactive) - roster 2026
-- Ejecutar con:
--   psql -h localhost -p 5432 -U dbdhub_user -d DBDHubDB -f Data/Seed/dbd_seed.sql
-- ============================================================

SET client_encoding = 'UTF8';
BEGIN;

-- Limpia registros de prueba generados por el escaneo de OWASP ZAP
-- y reinicia los contadores de identidad.
TRUNCATE TABLE public."Perks",
               public."MapKillers",
               public."Matches",
               public."Addons",
               public."Survivors",
               public."Maps",
               public."Killers"
    RESTART IDENTITY CASCADE;

-- ------------------------------------------------------------
-- KILLERS
-- ------------------------------------------------------------
INSERT INTO public."Killers"
    ("Nombre", "Poder", "Velocidad", "Dificultad", "Historia", "ImagenUrl", "Activo", "FechaCreacion")
VALUES
('The Trapper', 'Bear Traps', 4.6, 'Fácil', 'Evan MacMillan heredó el imperio industrial de su padre y lo convirtió en un laberinto de trampas y muerte.', NULL, true, now()),
('The Wraith', 'Wailing Bell', 4.6, 'Fácil', 'Philip Ojomo trabajaba en un desguace hasta que descubrió el oscuro destino de los cuerpos que allí llegaban.', NULL, true, now()),
('The Hillbilly', 'Chainsaw', 4.6, 'Medio', 'Max Thompson Jr. creció encerrado en una granja familiar, marcado por el abandono y la violencia.', NULL, true, now()),
('The Nurse', 'Spencer''s Last Breath', 3.85, 'Muy difícil', 'Sally Smithson perdió la razón tras la muerte de sus pacientes y ahora se desplaza entre planos de la realidad.', NULL, true, now()),
('The Shape', 'Evil Within', 4.6, 'Medio', 'Michael Myers es la personificación del mal puro, un asesino silencioso que acecha sin descanso.', NULL, true, now()),
('The Hag', 'Blackened Catalyst', 4.4, 'Muy difícil', 'Lisa Sherwood fue consumida por el canibalismo y ahora dibuja símbolos con los que acecha a sus presas.', NULL, true, now()),
('The Doctor', 'Carter''s Spark', 4.6, 'Difícil', 'Herman Carter realizó experimentos inhumanos en el instituto Léry y convirtió el dolor en electricidad.', NULL, true, now()),
('The Huntress', 'Hunting Hatchets', 4.4, 'Medio', 'Anna sobrevivió en los bosques de Rusia y caza a los intrusos con hachas arrojadizas.', NULL, true, now()),
('The Cannibal', 'Bubba''s Chainsaw', 4.6, 'Medio', 'Bubba Sawyer protege a su familia con una motosierra y una lealtad brutal.', NULL, true, now()),
('The Nightmare', 'Dream Demon', 4.6, 'Medio', 'Freddy Krueger acecha los sueños de sus víctimas y convierte el descanso en una pesadilla.', NULL, true, now()),
('The Pig', 'Jigsaw''s Baptism', 4.6, 'Difícil', 'Amanda Young heredó el legado de Jigsaw y somete a sus víctimas a pruebas mortales.', NULL, true, now()),
('The Clown', 'The Afterpiece Tonic', 4.6, 'Medio', 'Kenneth Chase colecciona el sufrimiento ajeno y lo combina con gases alucinógenos.', NULL, true, now()),
('The Spirit', 'Yamaoka''s Haunting', 4.4, 'Difícil', 'Rin Yamaoka fue asesinada por su padre y regresó como un espíritu vengativo.', NULL, true, now()),
('The Legion', 'Feral Frenzy', 4.6, 'Fácil', 'Frank Stone lidera una banda de adolescentes que encontraron en la violencia su verdadera naturaleza.', NULL, true, now()),
('The Plague', 'Vile Purge', 4.6, 'Difícil', 'Adiris fue una sacerdotisa babilónica que propagó una plaga como ofrenda a los dioses.', NULL, true, now()),
('The Ghost Face', 'Night Shroud', 4.6, 'Difícil', 'Danny Johnson es un periodista obsesionado con inmortalizar el miedo de sus victimas.', NULL, true, now()),
('The Demogorgon', 'Of the Abyss', 4.6, 'Difícil', 'Una criatura del Upside Down que atraviesa portales y devora todo lo que encuentra.', NULL, true, now()),
('The Oni', 'Yamaoka''s Wrath', 4.6, 'Muy difícil', 'Kazan Yamaoka, un samurái legendario, regresa convertido en un demonio sediento de sangre.', NULL, true, now()),
('The Deathslinger', 'The Redeemer', 4.6, 'Difícil', 'Caleb Quinn construyó un arpón letal para cobrar las deudas de quienes lo traicionaron.', NULL, true, now()),
('The Executioner', 'Rites of Judgement', 4.6, 'Muy difícil', 'Pyramid Head encarna el castigo eterno y persigue a los culpables con su gran cuchilla.', NULL, true, now()),
('The Blight', 'Blighted Corruption', 4.6, 'Muy difícil', 'Talbot Grimes consumió su propia serología y ahora se mueve a una velocidad imposible.', NULL, true, now()),
('The Twins', 'Blood Bond', 4.6, 'Muy difícil', 'Charlotte y Victor Deshayes comparten un vínculo monstruoso que los convierte en una sola amenaza.', NULL, true, now()),
('The Trickster', 'Showstopper', 4.6, 'Medio', 'Ji-Woon Hak, una estrella del K-pop, convierte sus conciertos en espectáculos de cuchillos.', NULL, true, now()),
('The Nemesis', 'T-Virus', 4.6, 'Difícil', 'El Nemesis es una bioarma de Umbrella diseñada para eliminar a los miembros de S.T.A.R.S.', NULL, true, now()),
('The Cenobite', 'Summons of Pain', 4.6, 'Difícil', 'Elliot Spencer, el Pinhead, abre portales al infierno con la configuración del lamento.', NULL, true, now()),
('The Artist', 'Birds of Torment', 4.6, 'Difícil', 'Carmina Mora encontró en los cuervos la forma de expresar el dolor que la consumía.', NULL, true, now()),
('The Onryō', 'Deluge of Fear', 4.4, 'Medio', 'Sadako Yamamura propaga su maldición a través de cintas de vídeo y pozos oscuros.', NULL, true, now()),
('The Dredge', 'Reign of Darkness', 4.6, 'Difícil', 'Una masa de pesadillas colectivas que se alimenta del miedo y se oculta en la oscuridad.', NULL, true, now()),
('The Mastermind', 'Virulent Bound', 4.6, 'Difícil', 'Albert Wesker, el villano de Resident Evil, persigue a los supervivientes con su fuerza sobrehumana.', NULL, true, now()),
('The Knight', 'Guardia Compagnia', 4.6, 'Medio', 'Tarhos Kovács comanda a sus guardias incluso después de la muerte, cazando en formación.', NULL, true, now()),
('The Skull Merchant', 'Eyes in the Sky', 4.6, 'Difícil', 'Adriana Imai utiliza drones y trampas tecnológicas para controlar el terreno de caza.', NULL, true, now()),
('The Singularity', 'Quantum Instantiation', 4.6, 'Difícil', 'HUX-A7-13 es una inteligencia artificial que reconstruye su cuerpo a partir de datos.', NULL, true, now()),
('The Xenomorph', 'Hidden Pursuit', 4.6, 'Difícil', 'El Xenomorph de Alien acecha desde los conductos y ataca con su cola y su lengua.', NULL, true, now()),
('The Good Guy', 'Playtime''s Over', 4.6, 'Medio', 'Charles Lee Ray transfirió su alma a un muñeco Chucky y continúa su matanza.', NULL, true, now()),
('The Unknown', 'UVX', 4.6, 'Medio', 'Una entidad amorfa que imita voces y se alimenta de la curiosidad de sus víctimas.', NULL, true, now()),
('The Lich', 'Vile Darkness', 4.6, 'Medio', 'Vecna, el archimago de Dungeons & Dragons, llega al reino de la Entidad con su magia oscura.', NULL, true, now()),
('The Dark Lord', 'Vampiric Shift', 4.6, 'Medio', 'Drácula, el señor de Castlevania, transforma su cuerpo y domina la noche.', NULL, true, now()),
('The Houndmaster', 'Scent of Blood', 4.6, 'Medio', 'Portia Maye caza junto a su sabueso, rastreando el rastro de sangre de los supervivientes.', NULL, true, now()),
('The Ghoul', 'One-Eyed Terror', 4.6, 'Medio', 'Ken Kaneki, el ghoul de Tokyo Ghoul, desata su kagune en una frenética cacería.', NULL, true, now()),
('The Animatronic', 'Fazbear''s Fright', 4.4, 'Medio', 'William Afton, atrapado en un traje animatrónico, arrastra el horror de Freddy Fazbear al reino.', NULL, true, now()),
('The Krasue', 'Unbodied Flesh', 4.6, 'Medio', 'Burong Sukapat, un espíritu del folclore tailandés, separa su cabeza del cuerpo para cazar.', NULL, true, now()),
('The First', 'Test Subject #001', 4.6, 'Difícil', 'Henry Creel, el primer sujeto de pruebas de Hawkins, manipula la mente de sus presas.', NULL, true, now()),
('The Slasher', 'Omnipresent Evil', 4.6, 'Medio', 'Jason Voorhees regresa del lago Crystal para continuar su interminable matanza.', NULL, true, now()),
('The Judgment', 'Will of the Gods', 4.4, 'Difícil', 'Una entidad divina que castiga la herejía y destierra a los supervivientes marcados.', NULL, true, now());

-- ------------------------------------------------------------
-- SURVIVORS
-- ------------------------------------------------------------
INSERT INTO public."Survivors"
    ("Nombre", "Historia", "Dificultad", "ImagenUrl", "Activo", "FechaCreacion")
VALUES
('Dwight Fairfield', 'Líder nato de un grupo de supervivientes, siempre dispuesto a ayudar a los demás.', 'Fácil', NULL, true, now()),
('Meg Thomas', 'Atleta universitaria que cambió las pistas de atletismo por la huida constante.', 'Fácil', NULL, true, now()),
('Claudette Morel', 'Botánica brillante que utiliza su conocimiento de las plantas para curar a sus aliados.', 'Fácil', NULL, true, now()),
('Jake Park', 'Solitario amante de la naturaleza que prefiere moverse entre las sombras.', 'Fácil', NULL, true, now()),
('Nea Karlsson', 'Traceur urbana experta en escapar por tejados y callejones.', 'Medio', NULL, true, now()),
('Laurie Strode', 'La hermana de Michael Myers, marcada por una noche de Halloween que nunca terminó.', 'Medio', NULL, true, now()),
('Ace Visconti', 'Apostador carismático que confía en su suerte para salir del reino de la Entidad.', 'Medio', NULL, true, now()),
('Bill Overbeck', 'Veterano de guerra que sacrificó su vida para salvar a sus compañeros.', 'Difícil', NULL, true, now()),
('Feng Min', 'Jugadora profesional que compite por sobrevivir como si fuera un torneo.', 'Medio', NULL, true, now()),
('Quentin Smith', 'Joven atormentado por Freddy Krueger que aprendió a no dormir.', 'Medio', NULL, true, now()),
('David Tapp', 'Detective obsesionado con resolver el caso que le costó todo.', 'Difícil', NULL, true, now()),
('David King', 'Boxeador inglés que resuelve los problemas con los puños y una lealtad inquebrantable.', 'Medio', NULL, true, now()),
('Kate Denson', 'Cantante de country que lleva la esperanza al reino con su música.', 'Fácil', NULL, true, now()),
('Adam Francis', 'Profesor universitario que analiza cada situación antes de actuar.', 'Difícil', NULL, true, now()),
('Jeff Johansen', 'Artista melancólico que retrata el horror que lo rodea.', 'Medio', NULL, true, now()),
('Jane Romero', 'Presentadora de televisión acostumbrada a los reflectores y a las despedidas.', 'Medio', NULL, true, now()),
('Ash Williams', 'El héroe de Evil Dead, experto en motosierras y frases ingeniosas.', 'Medio', NULL, true, now()),
('Nancy Wheeler', 'Periodista escolar decidida que investiga los misterios de Hawkins.', 'Medio', NULL, true, now()),
('Steve Harrington', 'El popular de Hawkins que aprendió a proteger a quienes quiere.', 'Medio', NULL, true, now()),
('Yui Kimura', 'Motociclista japonesa que corre más rápido que el miedo.', 'Difícil', NULL, true, now()),
('Zarina Kassir', 'Documentalista que expone las injusticias con su cámara.', 'Difícil', NULL, true, now()),
('Cheryl Mason', 'Superviviente de Silent Hill, acostumbrada a enfrentar horrores invisibles.', 'Difícil', NULL, true, now()),
('Felix Richter', 'Arquitecto alemán que planifica cada ruta de escape.', 'Medio', NULL, true, now()),
('Élodie Rakoto', 'Investigadora de lo paranormal que conoce los secretos de la Entidad.', 'Difícil', NULL, true, now()),
('Yun-Jin Lee', 'Productora musical coreana que prioriza su propia supervivencia.', 'Medio', NULL, true, now()),
('Jill Valentine', 'Miembro de S.T.A.R.S. y experta en sobrevivir a bioarmas.', 'Difícil', NULL, true, now()),
('Leon S. Kennedy', 'Policía novato de Raccoon City convertido en un superviviente experimentado.', 'Medio', NULL, true, now()),
('Mikaela Reid', 'Bruja moderna que canaliza hechizos para proteger a sus aliados.', 'Medio', NULL, true, now()),
('Yoichi Asakawa', 'Joven marcado por la maldición de Sadako y su cinta de vídeo.', 'Difícil', NULL, true, now()),
('Haddie Kaur', 'Exploradora de lo oculto que no teme enfrentarse a lo desconocido.', 'Difícil', NULL, true, now()),
('Ada Wong', 'Espía enigmática con recursos y contactos en los lugares más peligrosos.', 'Difícil', NULL, true, now()),
('Rebecca Chambers', 'Médica de S.T.A.R.S. experta en química y primeros auxilios.', 'Difícil', NULL, true, now()),
('Vittorio Toscano', 'Erudito italiano que ha estudiado los misterios de la Entidad durante siglos.', 'Difícil', NULL, true, now()),
('Thalita Lyra', 'Joven brasileña que lucha por mantener viva la esperanza.', 'Medio', NULL, true, now()),
('Renato Lyra', 'Hermano de Thalita, dispuesto a todo por protegerla.', 'Medio', NULL, true, now()),
('Gabriel Soma', 'Técnico espacial que sobrevivió a una catástrofe en el espacio.', 'Difícil', NULL, true, now()),
('Nicolas Cage', 'Actor de Hollywood que convierte el horror en un papel más.', 'Medio', NULL, true, now()),
('Ellen Ripley', 'Teniente de la Nostromo y experta en enfrentar xenomorfos.', 'Difícil', NULL, true, now()),
('Alan Wake', 'Escritor atrapado en sus propias historias de terror.', 'Medio', NULL, true, now()),
('Sable Ward', 'Joven que encontró en la oscuridad un refugio y una nueva identidad.', 'Medio', NULL, true, now()),
('Aestri Yazar', 'Bardo de Dungeons & Dragons que convierte la música en magia.', 'Difícil', NULL, true, now()),
('Baermar Uraz', 'Compañero de Aestri, un guerrero que no abandona a sus aliados.', 'Difícil', NULL, true, now()),
('Lara Croft', 'Arqueóloga y aventurera experta en sobrevivir a cualquier peligro.', 'Difícil', NULL, true, now()),
('Trevor Belmont', 'Cazador de monstruos de Castlevania que enfrenta la noche sin miedo.', 'Difícil', NULL, true, now()),
('Taurie Cain', 'Superviviente marcada por un pasado oscuro que intenta redimirse.', 'Medio', NULL, true, now()),
('Orela Rose', 'Paramédica que arriesga su vida para salvar a los demás.', 'Medio', NULL, true, now()),
('Vee Boonyasak', 'Joven tailandesa que sobrevive gracias a su ingenio.', 'Medio', NULL, true, now()),
('Rick Grimes', 'Sheriff y líder del grupo de The Walking Dead.', 'Difícil', NULL, true, now()),
('Michonne', 'Guerrera experta en el combate cuerpo a cuerpo y en la supervivencia.', 'Difícil', NULL, true, now()),
('Dustin Henderson', 'El cerebro del grupo de Hawkins, siempre con un plan.', 'Medio', NULL, true, now()),
('Eleven', 'Niña con habilidades psíquicas que lucha contra el Upside Down.', 'Difícil', NULL, true, now()),
('Kwon Tae-Young', 'Superviviente coreana que domina el flujo perfecto del combate.', 'Medio', NULL, true, now()),
('Aurora Stardotter', 'Superviviente celestial que desafía al juicio de los dioses.', 'Medio', NULL, true, now());

-- ------------------------------------------------------------
-- MAPS
-- ------------------------------------------------------------
INSERT INTO public."Maps"
    ("Nombre", "ImagenUrl", "Tipo", "OriginalMapId", "Activo", "FechaCreacion")
VALUES
('Coal Tower', NULL, 'Original', NULL, true, now()),
('Groaning Storehouse', NULL, 'Original', NULL, true, now()),
('Ironworks of Misery', NULL, 'Original', NULL, true, now()),
('Shelter Woods', NULL, 'Original', NULL, true, now()),
('Suffocation Pit', NULL, 'Original', NULL, true, now()),
('Azarov''s Resting Place', NULL, 'Original', NULL, true, now()),
('Blood Lodge', NULL, 'Original', NULL, true, now()),
('Gas Heaven', NULL, 'Original', NULL, true, now()),
('Wreckers'' Yard', NULL, 'Original', NULL, true, now()),
('Wretched Shop', NULL, 'Original', NULL, true, now()),
('Fractured Cowshed', NULL, 'Original', NULL, true, now()),
('Rancid Abattoir', NULL, 'Original', NULL, true, now()),
('Rotten Fields', NULL, 'Original', NULL, true, now()),
('Torment Creek', NULL, 'Original', NULL, true, now()),
('The Thompson House', NULL, 'Original', NULL, true, now()),
('Disturbed Ward', NULL, 'Original', NULL, true, now()),
('Father Campbell''s Chapel', NULL, 'Original', NULL, true, now()),
('The Pale Rose', NULL, 'Original', NULL, true, now()),
('Grim Pantry', NULL, 'Original', NULL, true, now()),
('Treatment Theatre', NULL, 'Original', NULL, true, now()),
('Mother''s Dwelling', NULL, 'Original', NULL, true, now()),
('Temple of Purgation', NULL, 'Original', NULL, true, now()),
('Family Residence', NULL, 'Original', NULL, true, now()),
('Sanctum of Wrath', NULL, 'Original', NULL, true, now()),
('Mount Ormond Resort', NULL, 'Original', NULL, true, now()),
('Ormond Lake Mine', NULL, 'Original', NULL, true, now()),
('Dead Dawg Saloon', NULL, 'Original', NULL, true, now()),
('Badham Preschool', NULL, 'Original', NULL, true, now()),
('The Game', NULL, 'Original', NULL, true, now()),
('The Underground Complex', NULL, 'Original', NULL, true, now()),
('Lampkin Lane', NULL, 'Original', NULL, true, now()),
('Midwich Elementary School', NULL, 'Original', NULL, true, now()),
('Raccoon City Police Station', NULL, 'Original', NULL, true, now()),
('Raccoon City Police Station East Wing', NULL, 'Original', NULL, true, now()),
('Raccoon City Police Station West Wing', NULL, 'Original', NULL, true, now()),
('Eyrie of Crows', NULL, 'Original', NULL, true, now()),
('Dead Sands', NULL, 'Original', NULL, true, now()),
('Forgotten Ruins', NULL, 'Original', NULL, true, now()),
('The Shattered Square', NULL, 'Original', NULL, true, now()),
('Garden of Joy', NULL, 'Original', NULL, true, now()),
('Greenville Square', NULL, 'Original', NULL, true, now()),
('Freddy Fazbear''s Pizza', NULL, 'Original', NULL, true, now()),
('Fallen Refuge', NULL, 'Original', NULL, true, now()),
('Toba Landing', NULL, 'Original', NULL, true, now()),
('Nostromo Wreckage', NULL, 'Original', NULL, true, now()),
('Trickster''s Delusion', NULL, 'Original', NULL, true, now());

-- Marca las dos alas del RPD como variantes del mapa principal
UPDATE public."Maps"
SET "Tipo" = 'Variante',
    "OriginalMapId" = (SELECT "Id" FROM public."Maps" WHERE "Nombre" = 'Raccoon City Police Station')
WHERE "Nombre" IN ('Raccoon City Police Station East Wing', 'Raccoon City Police Station West Wing');

-- ------------------------------------------------------------
-- ADDONS (3 por killer)
-- ------------------------------------------------------------
INSERT INTO public."Addons"
    ("Nombre", "Rareza", "Descripcion", "KillerId", "ImagenUrl", "Activo", "FechaCreacion")
SELECT a.nombre, a.rareza, a.descripcion, k."Id", NULL, true, now()
FROM (VALUES
    ('The Trapper', 'Trap Gag', 'Común', 'Aumenta ligeramente el tiempo que los supervivientes tardan en liberarse.'),
    ('The Trapper', 'Rusted Jaws', 'Poco común', 'Las trampas infligen hemorragia y dejan un rastro de sangre.'),
    ('The Trapper', 'Honing Stone', 'Muy raro', 'Las trampas pueden dejar a los supervivientes en estado moribundo.'),

    ('The Wraith', 'Blind Warrior', 'Común', 'Los supervivientes golpeados mientras se desvanece dejan un rastro visible.'),
    ('The Wraith', 'The Serpent - Soot', 'Raro', 'Permite atacar inmediatamente al salir del velo.'),
    ('The Wraith', 'Shadow Dance - Blood', 'Muy raro', 'Aumenta la velocidad de movimiento mientras está oculto.'),

    ('The Hillbilly', 'Dad''s Boots', 'Común', 'Reduce el tiempo de recarga de la motosierra.'),
    ('The Hillbilly', 'Rusted Chains', 'Poco común', 'La motosierra causa hemorragia al impactar.'),
    ('The Hillbilly', 'Thompson''s Mix', 'Muy raro', 'Otorga un impulso de velocidad tras una carga exitosa.'),

    ('The Nurse', 'Metal Spoon', 'Común', 'Reduce el tiempo de carga del parpadeo.'),
    ('The Nurse', 'Catatonic Boy''s Treasure', 'Raro', 'Aumenta el alcance del parpadeo.'),
    ('The Nurse', 'Fragile Wheeze', 'Muy raro', 'Permite un segundo parpadeo adicional.'),

    ('The Shape', 'Dead Rabbit', 'Común', 'Reduce la velocidad de carga de Evil Within.'),
    ('The Shape', 'Scratched Mirror', 'Raro', 'Permite ver las auras de los supervivientes a corta distancia.'),
    ('The Shape', 'Tombstone Piece', 'Muy raro', 'Permite matar directamente en el tercer nivel de Evil Within.'),

    ('The Hag', 'Bog Water', 'Común', 'Aumenta la velocidad de colocación de trampas.'),
    ('The Hag', 'Bloodied Mud', 'Raro', 'Las trampas activadas ralentizan a los supervivientes.'),
    ('The Hag', 'Mint Rag', 'Iridiscente', 'Permite teletransportarse a cualquier trampa del mapa.'),

    ('The Doctor', 'Discipline - Carter''s Notes', 'Común', 'Aumenta el rango de la descarga eléctrica.'),
    ('The Doctor', 'Restraint - Carter''s Notes', 'Raro', 'La descarga reduce la capacidad de reparación de los supervivientes.'),
    ('The Doctor', 'Iridescent King', 'Iridiscente', 'Aplica todos los estados de locura a la vez.'),

    ('The Huntress', 'Leather Loop', 'Común', 'Aumenta la velocidad de carga de las hachas.'),
    ('The Huntress', 'Wooden Fox', 'Raro', 'Reduce el tiempo de recarga tras lanzar un hacha.'),
    ('The Huntress', 'Iridescent Head', 'Iridiscente', 'Las hachas pueden derribar a un superviviente de un solo impacto.'),

    ('The Cannibal', 'Chili', 'Común', 'Aumenta la velocidad de movimiento durante el barrido de la motosierra.'),
    ('The Cannibal', 'Long Guide Bar', 'Raro', 'Aumenta el alcance del barrido.'),
    ('The Cannibal', 'Iridescent Chains', 'Iridiscente', 'La motosierra no se detiene al golpear a un superviviente.'),

    ('The Nightmare', 'Nancy''s Masterpiece', 'Común', 'Aumenta la velocidad de colocación de trampas de sueño.'),
    ('The Nightmare', 'Jump Rope', 'Raro', 'Reduce el tiempo de recarga de la proyección onírica.'),
    ('The Nightmare', 'Swing Chains', 'Muy raro', 'Aumenta la eficacia de las trampas de sueño.'),

    ('The Pig', 'Combat Straps', 'Común', 'Aumenta la velocidad de movimiento al agacharse.'),
    ('The Pig', 'Video Tape', 'Raro', 'Revela las auras de los supervivientes con trampa de oso.'),
    ('The Pig', 'Amanda''s Letter', 'Iridiscente', 'Los supervivientes comienzan con trampas de oso activas.'),

    ('The Clown', 'Ether 15 Vol%', 'Común', 'Aumenta la duración del gas alucinógeno.'),
    ('The Clown', 'Fingerless Parade Gloves', 'Raro', 'Aumenta la velocidad de recarga de las botellas.'),
    ('The Clown', 'Redhead''s Pinky Finger', 'Iridiscente', 'El gas provoca un estado de ceguera temporal.'),

    ('The Spirit', 'Yakuyoke Amulet', 'Común', 'Aumenta la velocidad de movimiento base.'),
    ('The Spirit', 'Bloody Hair Brooch', 'Raro', 'Reduce el tiempo de recarga de la fase.'),
    ('The Spirit', 'Iridescent Family Crest', 'Iridiscente', 'Permite atacar durante la fase de espíritu.'),

    ('The Legion', 'Smiley Face Pin', 'Común', 'Aumenta la velocidad de movimiento durante la frenesí.'),
    ('The Legion', 'Filthy Blade', 'Raro', 'La frenesí inflige hemorragia.'),
    ('The Legion', 'Iridescent Button', 'Iridiscente', 'La frenesí no se interrumpe al fallar un ataque.'),

    ('The Plague', 'Black Incense', 'Común', 'Aumenta la duración de la infección.'),
    ('The Plague', 'Incensed Ointment', 'Raro', 'Aumenta la velocidad de vómito.'),
    ('The Plague', 'Iridescent Seal', 'Iridiscente', 'La purga corrupta afecta a todos los supervivientes cercanos.'),

    ('The Ghost Face', 'Olive Branch', 'Común', 'Reduce el tiempo de recarga de Night Shroud.'),
    ('The Ghost Face', 'Walleye''s Matchbook', 'Raro', 'Aumenta la velocidad de movimiento mientras está oculto.'),
    ('The Ghost Face', 'Ghost Face Caught on Tape', 'Iridiscente', 'Permite derribar a un superviviente marcado al instante.'),

    ('The Demogorgon', 'Rotten Pumpkin', 'Común', 'Aumenta la velocidad de apertura de portales.'),
    ('The Demogorgon', 'Barb''s Glasses', 'Raro', 'Revela las auras de los supervivientes cerca de los portales.'),
    ('The Demogorgon', 'Iridescent Glass', 'Iridiscente', 'Permite atravesar portales instantáneamente.'),

    ('The Oni', 'Polished Cricket', 'Común', 'Aumenta la velocidad de absorción de sangre.'),
    ('The Oni', 'Splintered Hull', 'Raro', 'Reduce el tiempo de carga del ataque demoníaco.'),
    ('The Oni', 'Iridescent Demon Fang', 'Iridiscente', 'El ataque demoníaco derriba de un solo golpe.'),

    ('The Deathslinger', 'Honey Locust Thorn', 'Común', 'Aumenta la velocidad del proyectil.'),
    ('The Deathslinger', 'Warden''s Keys', 'Raro', 'Reduce el tiempo de recarga del arpón.'),
    ('The Deathslinger', 'Iridescent Coin', 'Iridiscente', 'El arpón atraviesa a los supervivientes y los hiere.'),

    ('The Executioner', 'Burning Man Painting', 'Común', 'Aumenta la velocidad de trazar surcos de castigo.'),
    ('The Executioner', 'Rusty Egg', 'Raro', 'Los supervivientes en el surco sufren hemorragia.'),
    ('The Executioner', 'Iridescent Seal of Metatron', 'Iridiscente', 'El ataque de juicio atraviesa paredes.'),

    ('The Blight', 'Cankerous Touch', 'Común', 'Aumenta la velocidad de recarga del impulso.'),
    ('The Blight', 'Shredded Notes', 'Raro', 'Reduce el coste de los rebotes.'),
    ('The Blight', 'Iridescent Blight Tag', 'Iridiscente', 'Otorga un impulso adicional tras un rebote exitoso.'),

    ('The Twins', 'Tiny Fingernails', 'Común', 'Aumenta la velocidad de Victor al saltar.'),
    ('The Twins', 'Toy Sword', 'Raro', 'Reduce el tiempo de recarga de Victor.'),
    ('The Twins', 'Iridescent Fetus', 'Iridiscente', 'Victor puede derribar a un superviviente de un solo salto.'),

    ('The Trickster', 'Tequila Sunrise', 'Común', 'Aumenta la velocidad de lanzamiento de cuchillos.'),
    ('The Trickster', 'Bloody Boa', 'Raro', 'Los cuchillos infligen hemorragia.'),
    ('The Trickster', 'Iridescent Photocard', 'Iridiscente', 'Los cuchillos atraviesan a los supervivientes.'),

    ('The Nemesis', 'Broken Recorder', 'Común', 'Aumenta la velocidad de infección del T-Virus.'),
    ('The Nemesis', 'Adrenaline Shot', 'Raro', 'Aumenta la velocidad de movimiento tras una mutación.'),
    ('The Nemesis', 'Iridescent Umbrella Badge', 'Iridiscente', 'El tentáculo puede atacar a varios supervivientes.'),

    ('The Cenobite', 'Liquefied Gore', 'Común', 'Aumenta la velocidad de lanzamiento de la cadena.'),
    ('The Cenobite', 'Torture Pillar', 'Raro', 'Reduce el tiempo de recarga de la configuración del lamento.'),
    ('The Cenobite', 'Iridescent Lament Configuration', 'Iridiscente', 'La caja aparece automáticamente ante los supervivientes.'),

    ('The Artist', 'Charcoal Stick', 'Común', 'Aumenta la duración de los cuervos.'),
    ('The Artist', 'Festering Carrion', 'Raro', 'Los cuervos infligen hemorragia al impactar.'),
    ('The Artist', 'Iridescent Feather', 'Iridiscente', 'Los cuervos revelan las auras de los supervivientes.'),

    ('The Onryō', 'Well Water', 'Común', 'Aumenta la velocidad de propagación de la maldición.'),
    ('The Onryō', 'Yoichi''s Fishing Net', 'Raro', 'Reduce el tiempo de recarga de la proyección.'),
    ('The Onryō', 'Iridescent Videotape', 'Iridiscente', 'La maldición se aplica a todos los supervivientes.'),

    ('The Dredge', 'Haddie''s Calendar', 'Común', 'Aumenta la velocidad de movimiento en la oscuridad.'),
    ('The Dredge', 'Fish and Chips', 'Raro', 'Revela las auras de los supervivientes cercanos a los armarios.'),
    ('The Dredge', 'Iridescent Wooden Plank', 'Iridiscente', 'Permite teletransportarse a cualquier armario del mapa.'),

    ('The Mastermind', 'Loose Cargo', 'Común', 'Aumenta la velocidad de carga de Virulent Bound.'),
    ('The Mastermind', 'Starved Beast', 'Raro', 'Reduce el tiempo de recarga tras un agarre.'),
    ('The Mastermind', 'Iridescent Uroboros Vial', 'Iridiscente', 'El agarre puede infectar a los supervivientes cercanos.'),

    ('The Knight', 'Chain Link', 'Común', 'Aumenta la velocidad de los guardias.'),
    ('The Knight', 'Map of the Realm', 'Raro', 'Reduce el tiempo de invocación de los guardias.'),
    ('The Knight', 'Iridescent Banner', 'Iridiscente', 'Los guardias permanecen más tiempo en el mapa.'),

    ('The Skull Merchant', 'Geek Grain', 'Común', 'Aumenta el rango de detección de los drones.'),
    ('The Skull Merchant', 'Infrared Upgrade', 'Raro', 'Los drones revelan las auras de los supervivientes.'),
    ('The Skull Merchant', 'Iridescent Circuit Board', 'Iridiscente', 'Los drones ralentizan a los supervivientes detectados.'),

    ('The Singularity', 'Emergency Override', 'Común', 'Aumenta la velocidad de escaneo.'),
    ('The Singularity', 'Crew Manifest', 'Raro', 'Reduce el tiempo de recarga de la instantánea cuántica.'),
    ('The Singularity', 'Iridescent Diagnostic Tool', 'Iridiscente', 'Permite teletransportarse a cualquier biopod del mapa.'),

    ('The Xenomorph', 'Molted Shell', 'Común', 'Aumenta la velocidad de movimiento en las alcantarillas.'),
    ('The Xenomorph', 'Emergency Evacuation Badge', 'Raro', 'Reduce el tiempo de recarga de la cola.'),
    ('The Xenomorph', 'Iridescent Xenomorph Egg', 'Iridiscente', 'La cola atraviesa a los supervivientes.'),

    ('The Good Guy', 'Good Guy Box', 'Común', 'Aumenta la velocidad de carga de Playtime''s Over.'),
    ('The Good Guy', 'Voodoo Doll', 'Raro', 'Reduce el tiempo de recarga de la embestida.'),
    ('The Good Guy', 'Iridescent Heart of Damballa', 'Iridiscente', 'Permite cambiar de cuerpo instantáneamente.'),

    ('The Unknown', 'Foreign Object', 'Común', 'Aumenta la velocidad de carga del UVX.'),
    ('The Unknown', 'Sporadic Spores', 'Raro', 'El UVX inflige hemorragia a los supervivientes afectados.'),
    ('The Unknown', 'Iridescent Axe Head', 'Iridiscente', 'El UVX puede derribar a un superviviente de un solo impacto.'),

    ('The Lich', 'Invisible Ink', 'Común', 'Aumenta la velocidad de lanzamiento de hechizos.'),
    ('The Lich', 'Spell Book', 'Raro', 'Reduce el tiempo de recarga de los hechizos.'),
    ('The Lich', 'Iridescent Book of Vile Darkness', 'Iridiscente', 'Permite lanzar dos hechizos seguidos.'),

    ('The Dark Lord', 'Sanguine Bond', 'Común', 'Aumenta la velocidad de absorción de sangre.'),
    ('The Dark Lord', 'Bloodmoon Chalice', 'Raro', 'Reduce el tiempo de recarga de la transformación.'),
    ('The Dark Lord', 'Iridescent Crimson Stone', 'Iridiscente', 'La transformación otorga un ataque adicional.'),

    ('The Houndmaster', 'Search Dog', 'Común', 'Aumenta la velocidad del sabueso.'),
    ('The Houndmaster', 'Hound''s Whistle', 'Raro', 'Reduce el tiempo de recarga de la orden de caza.'),
    ('The Houndmaster', 'Iridescent Hound Tag', 'Iridiscente', 'El sabueso revela las auras de los supervivientes.'),

    ('The Ghoul', 'Kagune', 'Común', 'Aumenta el alcance del ataque con kagune.'),
    ('The Ghoul', 'Rc Cell', 'Raro', 'Reduce el tiempo de recarga del ataque especial.'),
    ('The Ghoul', 'Iridescent Kakuja', 'Iridiscente', 'El ataque especial derriba de un solo golpe.'),

    ('The Animatronic', 'Party Hat', 'Común', 'Aumenta la velocidad de carga de Fazbear''s Fright.'),
    ('The Animatronic', 'Security Puppet', 'Raro', 'Revela las auras de los supervivientes cercanos.'),
    ('The Animatronic', 'Iridescent Endoskeleton', 'Iridiscente', 'El ataque no se interrumpe al fallar.'),

    ('The Krasue', 'Floating Entrails', 'Común', 'Aumenta la velocidad de movimiento en forma de cabeza.'),
    ('The Krasue', 'Krasue''s Comb', 'Raro', 'Reduce el tiempo de recarga de la separación corporal.'),
    ('The Krasue', 'Iridescent Body', 'Iridiscente', 'La cabeza puede atacar a distancia.'),

    ('The First', 'Test Subject Chip', 'Común', 'Aumenta la velocidad de carga de Test Subject #001.'),
    ('The First', 'Creel''s Journal', 'Raro', 'Reduce el tiempo de recarga de los poderes psíquicos.'),
    ('The First', 'Iridescent Mind Flayer', 'Iridiscente', 'Los poderes psíquicos afectan a todos los supervivientes.'),

    ('The Slasher', 'Hockey Mask', 'Común', 'Aumenta la velocidad de movimiento base.'),
    ('The Slasher', 'Machete Sheath', 'Raro', 'Reduce el tiempo de recarga de la embestida.'),
    ('The Slasher', 'Iridescent Jason Mask', 'Iridiscente', 'La embestida derriba a un superviviente de un solo golpe.'),

    ('The Judgment', 'Divine Light Shard', 'Común', 'Aumenta el alcance de la luz divina.'),
    ('The Judgment', 'Heresy Mark', 'Raro', 'Reduce el tiempo de recarga de la marca de herejía.'),
    ('The Judgment', 'Iridescent Halo', 'Iridiscente', 'Permite enviar al exilio a un superviviente marcado.')
) AS a(killer, nombre, rareza, descripcion)
JOIN public."Killers" k ON k."Nombre" = a.killer;

COMMIT;

-- ------------------------------------------------------------
-- Verificacion rapida
-- ------------------------------------------------------------
SELECT 'Killers' AS tabla, count(*) AS registros FROM public."Killers"
UNION ALL SELECT 'Survivors', count(*) FROM public."Survivors"
UNION ALL SELECT 'Maps', count(*) FROM public."Maps"
UNION ALL SELECT 'Addons', count(*) FROM public."Addons"
UNION ALL SELECT 'Perks', count(*) FROM public."Perks";
