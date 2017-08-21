-- ## DELETE ## --
PRINT 'removing data';
DELETE FROM Comments;
DELETE FROM SL_CommentState;
DELETE FROM Grades;
DELETE FROM Attendance;
DELETE FROM Lectures;
DELETE FROM Groups;
DELETE FROM Courses;
DELETE FROM SL_CourseStates;
DELETE FROM Teachers;
DELETE FROM Companies;
DELETE FROM Students;
DELETE FROM Users;
DELETE FROM SL_UserType;

-- ## INSERT ## --

--Tools:
--Imię i nazwisko: http://losownik.pl/imie/losuj/name-surname
--Adres: http://www.fakenamegenerator.com/gen-random-pl-pl.php
--Treść: http://www.lipsum.com/

PRINT 'Creating data';

-- SL_UserType --
INSERT INTO SL_UserType VALUES
(1, 'Administrator'),
(2, 'Student'),
(3, 'Firma'),
(4, 'Wykladowca'),
(5, 'WykladowcaStudent');

-- User --
INSERT INTO Users VALUES--SHA calculated using Sha512
(1,  1, 1, 0, 'a1',  'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Admin 1;                       a1   dupa
(2,  1, 1, 0, 'a2',  'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Admin 2;                       a2   dupa
(3,  2, 1, 0, 's1',  'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Student 1;                     s1   dupa
(4,  2, 1, 0, 's2',  'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Student 2;                     s2   dupa
(5,  2, 1, 1, 's3',  'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Student 3 (Banned);            s3   dupa
(6,  3, 1, 0, 'f1',  'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Firma 1;                       f1   dupa
(7,  3, 1, 0, 'f2',  'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Firma 2;                       f2   dupa
(8,  3, 1, 1, 'f3',  'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Firma 3 (Banned);              f3   dupa
(9,  4, 1, 0, 'w1',  'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Wykladowca 1;                  w1   dupa
(10, 4, 1, 0, 'w2',  'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Wykladowca 2;                  w2   dupa
(11, 4, 1, 1, 'w3',  'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Wykladowca 3 (Banned);         w3   dupa
(12, 5, 1, 0, 'ws1', 'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Wykladowco-student 1;          ws1  dupa
(13, 5, 1, 0, 'ws2', 'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC'),--Wykladowco-student 2;          ws2  dupa
(14, 5, 1, 1, 'ws3', 'ECBC63070B21965C18DD890B83CA2DA827951536128591DCFF357E2BF75052228C04257DCC8271B6CCF896763B9845E6029811038AEE55FFABD1DE8B8F99FCFC');--Wykladowco-student 3 (Banned); ws3  dupa

-- Student --
INSERT INTO Students VALUES
(3,  'Majchrzak', 'Marta', 'Granowska 120 60-101 Poznań', 'Majchrzak.Marta@void'),
(4,  'Borowski', 'Aleksander', 'Wrzosowa 129 20-203 Lublin', 'Borowski.Aleksander@void'),
(5,  'Kwiatkowska', 'Helena', 'Perlicza 143 02-893 Warszawa', 'Kwiatkowska.Helena@void'),
(12, 'Baran', 'Igor', 'Kłuszyńska 19 71-809 Szczecin', 'Baran.Igor@void'),
(13, 'Duda', 'Paulina', 'Poniatowskiego Józefa 36 15-199 Białystok', 'Duda.Paulina@void'),
(14, 'Kaczmarczyk', 'Agata', 'Ropczycka 64 61-316 Poznań', 'Kaczmarczyk.Agata@void');

-- Firma --
INSERT INTO Companies VALUES
(6, 'Aribu Consulting', 'Stawowa 133 50-018 Wrocław', 'email@Aribu.void', 'https://www.Aribu.website.void', 'Aliquam in eros diam. Duis in lacus ac est pretium aliquam non quis sapien. Nulla et bibendum lacus. Suspendisse gravida lectus a leo fringilla tempor. Nunc rhoncus pharetra nulla, et luctus dolor. Nullam aliquam felis odio, non accumsan dui convallis sed. Sed ac ante blandit, sagittis mauris eget, lobortis diam. In facilisis elit mi, non hendrerit justo efficitur id. Duis consequat urna pretium mattis sodales. Quisque mattis ligula magna, et tristique quam sagittis a. Quisque vehicula imperdiet mauris sit amet consequat. Vivamus in cursus purus.'),
(7, 'Yfube Szkoła Językowa', 'Rybitwy 75 02-806 Warszawa', 'email@Yfube.void', 'https://www.Yfube.website.void', 'Nullam sit amet purus nec odio dapibus blandit. In risus lacus, sollicitudin eu nisi tristique, accumsan pulvinar justo. Phasellus gravida nibh eget ornare consequat. Aliquam tempus fermentum felis, sit amet laoreet ipsum tempor consequat. Quisque et erat mauris. Aenean consequat rhoncus eros non ultrices. Aenean non cursus enim. Aenean commodo justo eget malesuada vulputate. Mauris augue leo, tincidunt consequat molestie quis, faucibus eu nibh. Proin id imperdiet tortor, id pulvinar nunc. Pellentesque condimentum lorem dignissim lobortis vehicula. Curabitur et egestas augue.'),
(8, 'Apimi Learning', 'Elektrownia 141 41-923 Bytom', 'email@Apimi.void', 'https://www.Apimi.website.void', 'Fusce sed diam in enim auctor vehicula a et mauris. Phasellus ac erat imperdiet, faucibus eros a, euismod turpis. Maecenas in ligula egestas, egestas mauris id, semper nisl. Aliquam aliquet risus risus, nec elementum lorem commodo in. Nunc ultrices a eros sit amet vulputate. Mauris pulvinar elit nisi, at euismod mi sagittis ut. Duis varius diam in arcu bibendum egestas. Nam nibh nulla, rhoncus in aliquam in, fringilla eu justo. Duis vel arcu efficitur, lobortis urna sit amet, congue urna. Morbi ut nisi ante.');

-- Wykladowca --
INSERT INTO Teachers VALUES
(9,  6, 'Majewska', 'Zuzanna', 'Aleksandrowska 5 91-154 Łódź', 'Majewska.Zuzanna@void', 'Doktor', 'https://zmajewska.website.void', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut aliquet sollicitudin dignissim. In pretium diam sed iaculis mollis. Cras accumsan lectus purus, non vehicula velit finibus vitae. Quisque massa ante, bibendum et eleifend a, condimentum dictum augue. Aliquam vitae molestie magna. Donec vehicula dictum tincidunt. Nulla mi est, consequat id sollicitudin ac, rutrum id nunc. In eget feugiat nisl. Suspendisse lobortis dolor eget dui rutrum, id vulputate sem ullamcorper.'),
(10,  7, 'Piotrowski', 'Wojciech', 'Masłowska 27 25-412 Kielce', 'Piotrowski.Wojciech@void', 'Magister', 'https://wpiotrowski.website.void', 'Cras gravida magna orci, nec malesuada nisi bibendum eu. Phasellus ac egestas neque. Suspendisse purus dolor, efficitur sollicitudin eleifend eget, luctus sit amet erat. Donec quis dui nec dolor gravida luctus. Proin at luctus risus, et suscipit est. Fusce suscipit pulvinar dolor, non finibus orci sodales non. In non lacinia velit. Vestibulum hendrerit, arcu at dignissim molestie, diam quam blandit neque, sit amet maximus enim erat quis nisi. Donec feugiat sagittis nisl et ornare. Quisque tincidunt, tortor eget tristique sodales, metus ipsum laoreet ligula, varius vehicula enim velit quis libero. Mauris quis dapibus risus. Praesent at quam id nulla rhoncus pharetra vel a dui. Nunc tincidunt ac purus non dapibus.'),
(11, 8, 'Kacper', 'Ostrowski', 'Czerniakowska 98 00-980 Warszawa', 'Ostrowski.Kacper@void', 'Inżynier', 'https://kostrowski.website.void', 'Suspendisse rutrum euismod eros, id posuere dolor rutrum vel. Mauris sed hendrerit libero. Vivamus dignissim scelerisque felis, ac fermentum mauris suscipit ut. Vivamus sed mattis magna. Donec ultrices ex quis libero vestibulum, vitae porta erat dictum. Integer ut nulla lacus. Fusce tincidunt urna eget accumsan aliquet.'),
(12, 6, 'Pietrzak', 'Igor', 'Horacego 13 80-299 Gdańsk', 'Pietrzak.Igor@void', 'Profesor', 'https://ipietrzak.website.void', 'Curabitur efficitur, nulla tincidunt imperdiet pharetra, nisi massa sodales nisi, et ultrices nibh felis a diam. Donec varius purus nec enim gravida, sit amet interdum enim faucibus. Duis sit amet volutpat purus, quis dignissim diam. Fusce et quam lectus. Aliquam luctus ante vel gravida pulvinar. Sed nec nisl eu eros auctor dapibus. Etiam vitae dolor purus. Maecenas ac mattis nunc. Integer dapibus consectetur interdum. Cras blandit purus gravida erat lacinia gravida. Curabitur luctus, nisl sit amet elementum lobortis, ante ipsum placerat ligula, sit amet consectetur ante justo sed eros.'),
(13, 7, 'Borkowski', 'Jan', 'Przybyszewskiego Stanisława 117 92-423 Łódź', 'Borkowski.Jan@void', 'Doktor', 'https://jborkowski.website.void', 'Etiam sagittis orci at metus dapibus, nec commodo metus bibendum. Aliquam in purus accumsan, iaculis felis vitae, dignissim libero. Nunc aliquet tempus eleifend. In ut augue nunc. Nullam congue risus quis nisi eleifend, non accumsan nulla convallis. Praesent enim elit, lacinia nec diam vitae, tincidunt auctor justo. Vestibulum mauris ipsum, ornare a auctor ac, suscipit et neque. Aliquam egestas ex sed dictum interdum. Ut efficitur tincidunt arcu, sed pellentesque tellus ultrices sit amet. Ut tristique eu purus eu posuere. Mauris non elit non neque pretium fringilla. Aliquam sollicitudin, quam ut porttitor sagittis, metus libero porttitor nisl, sit amet imperdiet lectus nibh eget ligula.'),
(14, 8, 'Krupa', 'Przemysław', 'Altanowa 42 43-316 Bielsko-Biała', 'Krupa.Przemysław@void', 'Magister', 'https://pkrupa.website.void', 'Suspendisse vel lacinia nisl. Pellentesque gravida gravida mauris. Vivamus egestas volutpat est id fermentum. Nulla sed enim viverra, ultricies ipsum quis, bibendum dolor. Aenean eu massa egestas erat sodales iaculis. Suspendisse non est sit amet dui sodales venenatis vitae id ipsum. Sed congue leo vel tincidunt auctor. Proin dictum dignissim libero at pharetra. Morbi suscipit, elit in convallis maximus, velit mi porttitor justo, eu placerat risus sem non nisl. In aliquam scelerisque commodo. Pellentesque maximus, enim sed varius placerat, mauris mi iaculis velit, sed consectetur lectus neque eu purus. Nulla ut erat massa.');

-- SL_CourseStates --
INSERT INTO SL_CourseStates VALUES
(1, 'Created'),
(2, 'Opened'),
(3, 'Started'),
(4, 'Finished');

-- Kurs --
INSERT INTO Courses VALUES
(1, 9, 6, 'Proin sit amet sem vitae mi venenatis sagittis eu vel est.', 4, 1, 'Curabitur varius ante imperdiet nulla varius lacinia. Pellentesque mauris erat, varius non sodales id, pretium vel nisi. Mauris aliquet ut dolor varius molestie. Praesent pharetra lobortis dui. Suspendisse aliquam vestibulum vehicula. Fusce ante dolor, fringilla eu nulla sed, ullamcorper iaculis sem. Morbi pulvinar massa mauris, vel accumsan risus rutrum sit amet. Sed venenatis volutpat tortor, vitae porta nisi dictum id. Donec arcu velit, faucibus et leo sed, euismod placerat lacus. Donec consequat pretium lorem ac accumsan. Suspendisse vel lobortis mauris. Curabitur iaculis vulputate metus, eu condimentum mauris tincidunt vel.', '20161001', '20161101'),
(2, 9, 6, 'Curabitur dignissim dui eu risus dapibus ultricies vitae id justo.', 4, 1, 'Etiam aliquet tempus ullamcorper. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris blandit tincidunt purus eu luctus. Maecenas at sem sit amet dui molestie mollis. Morbi nec diam nec magna interdum dictum. Nulla sit amet odio turpis. Vivamus porttitor et ipsum hendrerit rhoncus. Sed a nibh vulputate mauris bibendum sagittis.', '20161002', '20161102'),
(3, 10, 6, 'Ut pretium erat vel condimentum congue.', 4, 1, 'Nulla ac mollis lectus, id laoreet massa. Sed mollis tristique odio ac pellentesque. Cras hendrerit vulputate arcu, sed accumsan massa pulvinar eu. Vestibulum vitae faucibus ipsum. Integer congue tristique sem, ac condimentum dui maximus a. Vestibulum lectus est, hendrerit quis nunc sit amet, fermentum condimentum neque. Fusce nibh elit, imperdiet lacinia ultricies vel, posuere sit amet augue. Phasellus nisl magna, mollis eu ultrices non, molestie ac sem.', '20161003', '20161103'),
(4, 10, 7, 'Ut ac velit in dui faucibus semper.', 4, 1, 'Integer eu dapibus magna. Vivamus egestas arcu sem, consequat sodales elit dignissim id. Donec diam urna, ultrices eget tortor a, condimentum iaculis quam. Nam sollicitudin ultricies mauris sit amet commodo. Morbi cursus a risus at finibus. Nunc laoreet leo mollis lorem aliquam, sit amet venenatis risus malesuada. Nunc tellus mauris, sodales non leo quis, fermentum hendrerit urna. Maecenas feugiat finibus laoreet. Integer dolor ipsum, tempor at ligula non, commodo sodales dolor. Fusce vulputate tempus justo. In hac habitasse platea dictumst. Ut mi neque, viverra vitae vehicula quis, accumsan nec sem. Aliquam erat volutpat. Praesent eu erat id lorem tempor hendrerit.', '20161004', '20161104'),
(5, 12, 7, 'Pellentesque iaculis metus ac condimentum rutrum.', 4, 1, 'Mauris massa elit, luctus quis dapibus eu, viverra nec nisl. Mauris efficitur felis sed est bibendum tempus. Proin neque ex, sagittis nec iaculis ac, scelerisque in urna. Aenean faucibus lacus id tellus lacinia consectetur eu non ante. Nulla augue nisl, sollicitudin sed arcu sed, finibus vulputate orci. Vestibulum sed eleifend quam. Integer pulvinar sed enim sed semper. Cras porttitor quis sapien sed placerat. Nam ante libero, pharetra at dolor ultricies, vehicula ullamcorper purus. Integer venenatis libero ante, ut vulputate massa malesuada sed.', '20161005', '20161105'),
(6, 13, 7, 'Sed lobortis nisi in turpis vulputate aliquet sit amet quis ligula.', 4, 1, 'Nulla sed enim sem. Ut placerat aliquam mi, at lacinia felis. In imperdiet tortor euismod dolor elementum hendrerit. Etiam justo elit, condimentum nec dictum et, imperdiet quis erat. Cras molestie eget lectus sit amet rhoncus. Curabitur vulputate ac nunc vitae convallis. Aliquam ultrices egestas nulla, blandit hendrerit nulla. In mollis eget lacus sit amet ullamcorper. Fusce auctor magna eu sodales feugiat. Vestibulum eget libero orci. Mauris feugiat mauris dolor, id maximus ex mattis sit amet.', '20161006', '20161106');

-- Grupa --
INSERT INTO Groups VALUES --4 studentów, 6 kursów
(1,  3,  1),
(2,  3,  2),
(3,  3,  3),
(4,  3,  4),
(5,  3,  5),
(6,  3,  6),
(7,  4,  1),
(8,  4,  2),
(9,  4,  3),
(10, 4,  4),
(11, 4,  5),
(12, 4,  6),
(13, 12, 1),
(14, 12, 2),
(15, 12, 3),
(16, 12, 4),
(17, 12, 5),
(18, 12, 6),
(19, 13, 1),
(20, 13, 2),
(21, 13, 3),
(22, 13, 4),
(23, 13, 5),
(24, 13, 6);

-- Zajecia --
INSERT INTO Lectures VALUES
(1,  1, 9),
(2,  1, 9),
(3,  1, 9),
(4,  1, 9),
(5,  2, 9),
(6,  2, 9),
(7,  2, 10),
(8,  2, 10),
(9,  3, 10),
(10, 3, 10),
(11, 3, 10),
(12, 3, 10),
(13, 4, 12),
(14, 4, 12),
(15, 4, 12),
(16, 4, 12),
(17, 5, 12),
(18, 5, 12),
(19, 5, 13),
(20, 5, 13),
(21, 6, 13),
(22, 6, 13),
(23, 6, 13),
(24, 6, 13);

-- Obecnosci --
INSERT INTO Attendance VALUES
(1,  3,  1),
(1,  4,  1),
(1,  12, 1),
(1,  13, 1),
(2,  3,  1),
(2,  4,  1),
(2,  12, 1),
(2,  13, 1),
(3,  3,  1),
(3,  4,  1),
(3,  12, 1),
(3,  13, 1),
(4,  3,  1),
(4,  4,  1),
(4,  12, 1),
(4,  13, 1),
(5,  3,  1),
(5,  4,  1),
(5,  12, 1),
(5,  13, 1),
(6,  3,  1),
(6,  4,  1),
(6,  12, 1),
(6,  13, 1),
(7,  3,  1),
(7,  4,  1),
(7,  12, 1),
(7,  13, 1),
(8,  3,  1),
(8,  4,  1),
(8,  12, 1),
(8,  13, 1),
(9,  3,  1),
(9,  4,  1),
(9,  12, 1),
(9,  13, 1),
(10, 3,  1),
(10, 4,  1),
(10, 12, 1),
(10, 13, 1),
(11, 3,  1),
(11, 4,  1),
(11, 12, 1),
(11, 13, 1),
(12, 3,  1),
(12, 4,  1),
(12, 12, 1),
(12, 13, 1),
(13, 3,  1),
(13, 4,  1),
(13, 12, 1),
(13, 13, 1),
(14, 3,  1),
(14, 4,  1),
(14, 12, 1),
(14, 13, 1),
(15, 3,  1),
(15, 4,  1),
(15, 12, 1),
(15, 13, 1),
(16, 3,  1),
(16, 4,  1),
(16, 12, 1),
(16, 13, 1),
(17, 3,  1),
(17, 4,  1),
(17, 12, 1),
(17, 13, 1),
(18, 3,  1),
(18, 4,  1),
(18, 12, 1),
(18, 13, 1),
(19, 3,  1),
(19, 4,  1),
(19, 12, 1),
(19, 13, 1),
(20, 3,  1),
(20, 4,  1),
(20, 12, 1),
(20, 13, 1),
(21, 3,  1),
(21, 4,  1),
(21, 12, 1),
(21, 13, 1),
(22, 3,  1),
(22, 4,  1),
(22, 12, 1),
(22, 13, 1),
(23, 3,  1),
(23, 4,  1),
(23, 12, 1),
(23, 13, 1),
(24, 3,  1),
(24, 4,  1),
(24, 12, 1),
(24, 13, 1);

-- Oceny --
INSERT INTO Grades VALUES
(1,  3,  1, 5.0, '20161101', 9,  'Etiam sit amet leo id dolor faucibus pellentesque.'),
(2,  4,  1, 4.5, '20161101', 9,  'Suspendisse varius nisi non massa rutrum, nec mattis elit convallis.'),
(3,  12, 1, 4.0, '20161101', 9,  'Mauris sit amet eros sit amet nibh auctor semper.'),
(4,  13, 1, 3.5, '20161101', 9,  'Sed non ante in velit mollis rhoncus.'),
(5,  3,  2, 3.0, '20161102', 9,  'Nullam congue turpis luctus sodales egestas.'),
(6,  4,  2, 5.0, '20161102', 9,  'Nam auctor enim sollicitudin massa interdum, non ultricies leo feugiat.'),
(7,  12, 2, 4.5, '20161102', 9,  'Donec et eros imperdiet arcu pulvinar viverra non ac nisl.'),
(8,  13, 2, 4.0, '20161102', 9,  'Etiam sed ante a lacus venenatis laoreet at eu diam.'),
(9,  3,  3, 3.5, '20161103', 10, 'Donec a velit placerat, mattis lacus dignissim, feugiat mauris.'),
(10, 4,  3, 3.0, '20161103', 10, 'Donec tincidunt purus ac ligula sollicitudin, vitae porta dui eleifend.'),
(11, 12, 3, 5.0, '20161103', 10, 'Fusce in urna aliquam lectus sodales blandit quis gravida lorem.'),
(12, 13, 3, 4.5, '20161103', 10, 'Suspendisse non mi in metus vestibulum iaculis.'),
(13, 3,  4, 4.0, '20161104', 10, 'Nulla eget lacus posuere neque ultrices tristique.'),
(14, 4,  4, 3.5, '20161104', 10, 'Sed ut lorem ullamcorper, condimentum risus a, ullamcorper felis.'),
(15, 12, 4, 3.0, '20161104', 10, 'Donec venenatis nibh id sapien sagittis vehicula.'),
(16, 13, 4, 5.0, '20161104', 10, 'Fusce sed arcu non urna cursus posuere quis nec urna.'),
(17, 3,  5, 4.5, '20161105', 12, 'Nunc ac nisl ac turpis volutpat vestibulum sit amet imperdiet turpis.'),
(18, 4,  5, 4.0, '20161105', 12, 'Sed viverra justo consequat euismod pulvinar.'),
(19, 12, 5, 3.5, '20161105', 12, 'Cras blandit erat sed lectus bibendum rhoncus.'),
(20, 13, 5, 3.0, '20161105', 12, 'Fusce id magna in nunc ultricies lobortis.'),
(21, 3,  6, 5.0, '20161106', 13, 'Nullam et nunc iaculis massa vestibulum efficitur quis ultrices ante.'),
(22, 4,  6, 4.5, '20161106', 13, 'Duis blandit urna at diam convallis dignissim.'),
(23, 12, 6, 4.0, '20161106', 13, 'Proin eget lorem a est dictum pulvinar in in erat.'),
(24, 13, 6, 3.5, '20161106', 13, 'Sed convallis quam eget finibus laoreet.');

-- SL_StanKomentarza --
INSERT INTO SL_CommentState VALUES
(1, 'Negatywny'),
(2, 'Neutralny'),
(3, 'Pozytywny'),
(4, 'Zablokowany');

-- Komentarze --
INSERT INTO Comments VALUES
(1,  3,  1, '20171111', 'In sapien leo, scelerisque et aliquam et, ultrices non tellus. Maecenas mi ligula, facilisis vitae dolor non, cursus cursus mi. Aliquam faucibus ex quis enim semper placerat. Quisque ornare et massa eget faucibus. Aenean suscipit lectus sem, cursus placerat orci commodo et. Fusce efficitur risus volutpat nisi commodo ornare. Aenean non tempor augue. Duis sed tortor est. Fusce sed urna a risus venenatis tristique. Sed dignissim pretium viverra. Ut at dui elementum, fermentum turpis nec, faucibus eros.', 1),
(2,  4,  1, '20171111', 'Duis ac neque a turpis elementum imperdiet. In hac habitasse platea dictumst. Proin tincidunt condimentum tincidunt. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Proin et aliquam metus. Donec egestas efficitur semper. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Quisque scelerisque sapien orci, vel faucibus lorem fermentum et. Quisque a blandit elit, eu maximus arcu.', 2),
(3,  12, 1, '20171111', 'In accumsan cursus arcu, eget luctus libero luctus sagittis. Curabitur ac pellentesque dolor, in laoreet arcu. Fusce non augue dapibus, imperdiet lectus ac, bibendum lectus. Mauris nec arcu ac dui cursus tincidunt vel vitae sem. Curabitur arcu libero, scelerisque a turpis in, accumsan semper ligula. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean condimentum eros ultrices erat cursus eleifend.', 3),
(4,  13, 1, '20171111', 'Vivamus auctor dictum nisl non eleifend. Aenean vitae malesuada quam. Vestibulum eros turpis, semper venenatis feugiat non, euismod ac ipsum. Integer porta dignissim ligula vel euismod. Donec in semper dui. Nunc et erat at metus ornare pellentesque vel a nisi. Nam quis vestibulum lacus. Nulla facilisi. Suspendisse nisi leo, ornare quis laoreet eu, feugiat et orci. Nam eget eros ligula. Nunc mattis hendrerit leo eget sagittis. Quisque quis neque vehicula, dignissim lorem at, facilisis ligula. Etiam vestibulum nulla ac lacinia consequat. Aenean vel iaculis ex, ut convallis elit. In consequat massa quis placerat tempor.', 3),
(5,  3,  2, '20171112', 'Aliquam mauris erat, imperdiet ut placerat in, volutpat rhoncus velit. Suspendisse ac feugiat risus, eget maximus quam. Curabitur ultrices porttitor mauris non consectetur. Pellentesque semper quis mi mollis fermentum. Aenean tincidunt lacus nec lectus pretium, non cursus enim faucibus. Phasellus efficitur aliquet ornare. Donec et odio eget lorem sollicitudin fermentum vel at enim. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. In hac habitasse platea dictumst. Praesent urna augue, mattis quis auctor a, fermentum sed justo. Vivamus pharetra ut lacus eget efficitur.', 2),
(6,  4,  2, '20171112', 'Praesent vitae rutrum velit. Cras magna lacus, ornare at mauris at, rhoncus imperdiet mi. Sed mauris nibh, iaculis at rhoncus sed, volutpat id massa. Nulla aliquam mi sit amet laoreet dignissim. Donec congue lorem eu quam maximus ultrices. Vestibulum a tellus in augue tempor efficitur. Pellentesque turpis ipsum, pharetra nec enim in, maximus interdum lacus.', 3),
(7,  12, 2, '20171112', 'Vestibulum at hendrerit elit, pharetra mollis velit. Duis vel ligula ultrices, euismod sapien tincidunt, rutrum ante. Vivamus cursus tempor quam, sit amet vestibulum eros porta a. Donec luctus maximus tortor at rhoncus. Mauris porta auctor nibh sed hendrerit. Nam nisi felis, vestibulum ut maximus nec, elementum ut sem. Nulla blandit ac diam eu hendrerit. Aenean sit amet pulvinar ex, sed semper diam. Pellentesque vitae imperdiet magna.', 2),
(8,  13, 2, '20171112', 'Ut erat metus, tristique a bibendum in, venenatis in tellus. Nam non erat fringilla ante condimentum porttitor. Etiam sit amet neque dui. Nunc sit amet quam a metus cursus placerat. Nulla hendrerit nulla ac elementum pharetra. Phasellus pellentesque diam eget tempus dignissim. Vivamus volutpat sem a consequat vulputate.', 1),
(9,  3,  3, '20171113', 'Sed a erat felis. Mauris vel ligula nulla. Morbi vehicula ligula nec tincidunt imperdiet. Duis egestas rutrum ex, in scelerisque orci vulputate vel. Quisque vel quam eu ex venenatis tristique quis eget massa. Donec a augue eget justo imperdiet ullamcorper. Suspendisse id vulputate odio.', 2),
(10, 4,  3, '20171113', 'In tincidunt urna tellus. Etiam at ornare dui. Pellentesque aliquam bibendum nibh vitae blandit. Integer euismod et lectus non egestas. Vivamus iaculis bibendum fringilla. Nulla non orci lacus. Phasellus laoreet nisl ante, vel placerat purus vestibulum at. Donec ornare velit a finibus posuere.', 4),
(11, 12, 3, '20171113', 'Etiam varius lobortis consequat. Sed porta elit ac consequat ultrices. Cras ac dapibus nibh. Fusce dignissim pharetra accumsan. Integer luctus molestie bibendum. Fusce quis dictum mauris. Aenean nec cursus ligula. Integer ex leo, imperdiet eget ante quis, euismod faucibus tortor. Nullam eros orci, molestie vel erat sollicitudin, malesuada ultricies lacus.', 2),
(12, 13, 3, '20171113', 'Ut sed ante maximus, ullamcorper magna id, rutrum odio. Mauris aliquet mollis turpis, eget porta tortor consequat sit amet. Sed sagittis lectus erat, et tincidunt ligula vestibulum eu. Nam diam sapien, dapibus non sapien vel, malesuada laoreet massa. Vestibulum mollis magna sed urna porta, nec tincidunt lacus elementum. Aliquam congue justo a mauris ornare facilisis. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos.', 4),
(13, 3,  4, '20171114', 'Vivamus elementum quam vel massa viverra, vitae sagittis sapien feugiat. Nulla maximus, elit at maximus lobortis, magna magna posuere nulla, sit amet interdum mi ante sed velit. Vestibulum ac varius ipsum. Aliquam et consequat velit. Praesent eu molestie lectus. Donec vehicula sollicitudin lacus eu tincidunt. Nunc ornare cursus mi, vel hendrerit risus. Phasellus volutpat dui ac nisi consectetur accumsan.', 2),
(14, 4,  4, '20171114', 'Quisque scelerisque pretium vestibulum. Curabitur sit amet faucibus diam. Vestibulum dui turpis, faucibus sit amet arcu quis, bibendum auctor felis. Fusce suscipit volutpat mauris, auctor maximus lacus pretium vel. Vestibulum in aliquet ligula, sit amet fermentum tortor. Nulla facilisis blandit fringilla. Mauris ac libero justo.', 3),
(15, 12, 4, '20171114', 'Nulla non est sed ligula luctus hendrerit. Ut nulla dui, maximus vitae lorem et, congue aliquet quam. Etiam vulputate nisl eget libero tincidunt suscipit. Nunc tristique condimentum nibh eget ullamcorper. Aenean auctor turpis vel est dapibus, at tincidunt mi ultrices. Ut vitae nunc augue. Sed et tristique est. Ut ac sodales urna. Donec condimentum orci interdum ipsum mattis, semper placerat lacus mollis.', 3),
(16, 13, 4, '20171114', 'Etiam tempus non justo vitae commodo. Duis id hendrerit nulla, at fringilla velit. Ut venenatis eros euismod nisl efficitur sollicitudin. Aliquam lectus augue, viverra imperdiet lectus eu, hendrerit ullamcorper tellus. Morbi id sollicitudin ante, nec efficitur lectus. Cras condimentum, sem a facilisis egestas, enim dolor eleifend urna, nec gravida elit metus eget nisi.', 2),
(17, 3,  5, '20171115', 'Phasellus mattis, dolor vel consequat auctor, enim nisl placerat ante, ac tristique ipsum metus sed purus. Praesent sit amet massa vel nulla posuere pellentesque ac ac odio. Fusce at tempor nisi. Aliquam vitae commodo urna. Aliquam malesuada leo in massa tristique varius at quis lorem.', 1),
(18, 4,  5, '20171115', 'Sed mollis neque finibus, lobortis neque at, aliquam mauris. Mauris ultrices justo quis nibh egestas ultricies. Etiam congue nulla nunc, sed porttitor massa sagittis vel. Aenean lobortis placerat odio vel convallis. Cras luctus nunc at arcu semper finibus. Fusce vulputate placerat tellus a ullamcorper. In scelerisque ut orci vel pellentesque. Suspendisse mollis maximus condimentum. Vestibulum bibendum quam ut ex cursus sollicitudin.', 1),
(19, 12, 5, '20171115', 'Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nunc eu auctor felis, vitae rutrum libero. Nam finibus feugiat quam non posuere. Sed dictum massa tellus, eu eleifend ante malesuada eu. Aenean et pulvinar felis, quis dignissim lectus. Proin mollis finibus tempor. Suspendisse euismod quam ac pretium lobortis. Vivamus sed accumsan sem.', 2),
(20, 13, 5, '20171115', 'Etiam mattis maximus turpis, non rhoncus leo mollis ut. Curabitur vitae purus imperdiet, elementum arcu a, interdum purus. Cras vitae neque sagittis, accumsan enim eget, suscipit elit. Integer eu vestibulum nulla. Phasellus ullamcorper nisi non sollicitudin pulvinar. Donec ut massa quam. Morbi ac mi mollis, fermentum orci ac, accumsan ipsum.', 3),
(21, 3,  6, '20171116', 'Donec pretium felis nibh, nec consectetur nunc blandit non. Phasellus luctus enim at ullamcorper facilisis. Donec vitae neque id urna molestie lacinia. Suspendisse viverra nisi a nunc elementum, vitae porttitor sapien porta. Cras eu imperdiet nunc. Suspendisse tincidunt, metus quis semper mollis, nunc lorem commodo mauris, eu accumsan erat turpis vitae tortor. Mauris ut molestie augue. Pellentesque sit amet viverra elit.', 2),
(22, 4,  6, '20171116', 'Curabitur nisi metus, bibendum eu elementum eu, mattis vel diam. Proin elementum in metus eu vestibulum. Cras bibendum tellus sem, id lobortis sem molestie sit amet. Suspendisse mollis id nisi eget finibus. Nullam tempus nisl at nisi mollis varius. Vestibulum placerat purus sed mi condimentum dapibus. Donec eu porttitor risus.', 1),
(23, 12, 6, '20171116', 'Mauris finibus est sit amet magna porttitor, ac sagittis purus luctus. Aliquam consequat facilisis orci, ut interdum orci eleifend quis. Etiam elementum magna a mi cursus auctor. Phasellus quis lectus finibus, egestas arcu ac, suscipit neque. Donec aliquet nibh risus, et lobortis ligula viverra sed. Donec mattis felis ac nunc ornare, vitae hendrerit odio mattis. Sed convallis leo turpis, id pellentesque justo ultricies vitae.', 3),
(24, 13, 6, '20171116', 'In ut elit odio. Nunc nec risus vitae elit convallis condimentum. Etiam justo sem, molestie eget posuere sed, porta at diam. Maecenas tempus, magna at fermentum congue, mauris orci auctor nisi, at tempor leo nibh nec lectus. Aliquam consequat consequat facilisis. Morbi porta id turpis ac iaculis. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Maecenas elementum aliquam gravida.', 2);