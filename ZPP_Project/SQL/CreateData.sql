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
DELETE FROM EF_UserLogins;
DELETE FROM EF_UserClaims;
DELETE FROM EF_UserRoles;
DELETE FROM Users;
DELETE FROM SL_UserType;


PRINT 'reseeding'
DBCC CHECKIDENT (Comments, RESEED, 0);
DBCC CHECKIDENT (Grades, RESEED, 0);
DBCC CHECKIDENT (Attendance, RESEED, 0);
DBCC CHECKIDENT (Lectures, RESEED, 0);
DBCC CHECKIDENT (Groups, RESEED, 0);
DBCC CHECKIDENT (Courses, RESEED, 0);
DBCC CHECKIDENT (Teachers, RESEED, 0);
DBCC CHECKIDENT (Companies, RESEED, 0);
DBCC CHECKIDENT (Students, RESEED, 0);
DBCC CHECKIDENT (EF_UserClaims, RESEED, 0);
DBCC CHECKIDENT (Users, RESEED, 0);
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
(3, 'Company'),
(4, 'Teacher'),
(5, 'TeacherStudent');

-- User --
INSERT INTO Users VALUES--SHA calculated using Sha512
(1, 1, 0, 'a1',  'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'admin1@zppproject.void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 0, NULL, 0),						--Admin 1;						 a1   asdf
(1, 1, 0, 'a2',  'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'admin2@zppproject.void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, NULL, 0),						--Admin 2;						 a2   asdf
(2, 1, 0, 's1',  'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'Majchrzak.Marta@void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, NULL, 0),						--Student 1;					 s1   asdf
(2, 1, 0, 's2',  'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'Borowski.Aleksander@void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, NULL, 0),					--Student 2;    				 s2   asdf
(2, 1, 1, 's3',  'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'Kwiatkowska.Helena@void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, '2099-12-31 23:59:59', 0),	--Student 3 (Banned);			 s3   asdf
(3, 1, 0, 'f1',  'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'email@Aribu.void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, NULL, 0),							--Firma 1;						 f1   asdf
(3, 1, 0, 'f2',  'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'email@Yfube.void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, NULL, 0),							--Firma 2;  					 f2   asdf
(3, 1, 1, 'f3',  'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'email@Apimi.void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, '2099-12-31 23:59:59', 0),			--Firma 3 (Banned);				 f3   asdf
(4, 1, 0, 'w1',  'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'Majewska.Zuzanna@void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, NULL, 0),						--Wykladowca 1;             	 w1   asdf
(4, 1, 0, 'w2',  'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'Piotrowski.Wojciech@void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, NULL, 0),					--Wykladowca 2;             	 w2   asdf
(4, 1, 1, 'w3',  'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'Ostrowski.Kacper@void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, '2099-12-31 23:59:59', 0),		--Wykladowca 3 (Banned);		 w3   asdf
(5, 1, 0, 'ws1', 'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'Baran.Igor@void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, NULL, 0),							--Wykladowco-student 1;          ws1  asdf
(5, 1, 0, 'ws2', 'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'Duda.Paulina@void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, NULL, 0),							--Wykladowco-student 2;          ws2  asdf
(5, 1, 1, 'ws3', 'D507CD982387FAD9445F7E356E8B2661F19CCDD111BBBDFD6910FFCA6253EA2FEA0DA3C2DBFA8C1DACAF2F1F268DC0F6FC732A1860850135D17D5E65093EF6CB', 'Kaczmarczyk.Agata@void', '09d2f0dc-0f40-4dfa-a8b3-8b9fd40a26f7', NULL, 0, 0, 1, '2099-12-31 23:59:59', 0);	--Wykladowco-student 3 (Banned); ws3  asdf

-- Student --
INSERT INTO Students VALUES
(3,  'Majchrzak', 'Marta', 'Granowska 120 60-101 Poznań'),
(4,  'Borowski', 'Aleksander', 'Wrzosowa 129 20-203 Lublin'),
(5,  'Kwiatkowska', 'Helena', 'Perlicza 143 02-893 Warszawa'),
(12, 'Baran', 'Igor', 'Kłuszyńska 19 71-809 Szczecin'),
(13, 'Duda', 'Paulina', 'Poniatowskiego Józefa 36 15-199 Białystok'),
(14, 'Kaczmarczyk', 'Agata', 'Ropczycka 64 61-316 Poznań');

-- Firma --
INSERT INTO Companies VALUES
(6, 'Aribu Consulting', 'Stawowa 133 50-018 Wrocław', 'email@Aribu.void', 'https://www.Aribu.website.void', 'Aliquam in eros diam. Duis in lacus ac est pretium aliquam non quis sapien. Nulla et bibendum lacus. Suspendisse gravida lectus a leo fringilla tempor. Nunc rhoncus pharetra nulla, et luctus dolor. Nullam aliquam felis odio, non accumsan dui convallis sed. Sed ac ante blandit, sagittis mauris eget, lobortis diam. In facilisis elit mi, non hendrerit justo efficitur id. Duis consequat urna pretium mattis sodales. Quisque mattis ligula magna, et tristique quam sagittis a. Quisque vehicula imperdiet mauris sit amet consequat. Vivamus in cursus purus.'),
(7, 'Yfube Szkoła Językowa', 'Rybitwy 75 02-806 Warszawa', 'email@Yfube.void', 'https://www.Yfube.website.void', 'Nullam sit amet purus nec odio dapibus blandit. In risus lacus, sollicitudin eu nisi tristique, accumsan pulvinar justo. Phasellus gravida nibh eget ornare consequat. Aliquam tempus fermentum felis, sit amet laoreet ipsum tempor consequat. Quisque et erat mauris. Aenean consequat rhoncus eros non ultrices. Aenean non cursus enim. Aenean commodo justo eget malesuada vulputate. Mauris augue leo, tincidunt consequat molestie quis, faucibus eu nibh. Proin id imperdiet tortor, id pulvinar nunc. Pellentesque condimentum lorem dignissim lobortis vehicula. Curabitur et egestas augue.'),
(8, 'Apimi Learning', 'Elektrownia 141 41-923 Bytom', 'email@Apimi.void', 'https://www.Apimi.website.void', 'Fusce sed diam in enim auctor vehicula a et mauris. Phasellus ac erat imperdiet, faucibus eros a, euismod turpis. Maecenas in ligula egestas, egestas mauris id, semper nisl. Aliquam aliquet risus risus, nec elementum lorem commodo in. Nunc ultrices a eros sit amet vulputate. Mauris pulvinar elit nisi, at euismod mi sagittis ut. Duis varius diam in arcu bibendum egestas. Nam nibh nulla, rhoncus in aliquam in, fringilla eu justo. Duis vel arcu efficitur, lobortis urna sit amet, congue urna. Morbi ut nisi ante.');

-- Wykladowca --
INSERT INTO Teachers VALUES
(9,  1, 'Majewska', 'Zuzanna', 'Aleksandrowska 5 91-154 Łódź', 'Doktor', 'https://zmajewska.website.void', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut aliquet sollicitudin dignissim. In pretium diam sed iaculis mollis. Cras accumsan lectus purus, non vehicula velit finibus vitae. Quisque massa ante, bibendum et eleifend a, condimentum dictum augue. Aliquam vitae molestie magna. Donec vehicula dictum tincidunt. Nulla mi est, consequat id sollicitudin ac, rutrum id nunc. In eget feugiat nisl. Suspendisse lobortis dolor eget dui rutrum, id vulputate sem ullamcorper.'),
(10, 2, 'Piotrowski', 'Wojciech', 'Masłowska 27 25-412 Kielce', 'Magister', 'https://wpiotrowski.website.void', 'Cras gravida magna orci, nec malesuada nisi bibendum eu. Phasellus ac egestas neque. Suspendisse purus dolor, efficitur sollicitudin eleifend eget, luctus sit amet erat. Donec quis dui nec dolor gravida luctus. Proin at luctus risus, et suscipit est. Fusce suscipit pulvinar dolor, non finibus orci sodales non. In non lacinia velit. Vestibulum hendrerit, arcu at dignissim molestie, diam quam blandit neque, sit amet maximus enim erat quis nisi. Donec feugiat sagittis nisl et ornare. Quisque tincidunt, tortor eget tristique sodales, metus ipsum laoreet ligula, varius vehicula enim velit quis libero. Mauris quis dapibus risus. Praesent at quam id nulla rhoncus pharetra vel a dui. Nunc tincidunt ac purus non dapibus.'),
(11, 3, 'Kacper', 'Ostrowski', 'Czerniakowska 98 00-980 Warszawa', 'Inżynier', 'https://kostrowski.website.void', 'Suspendisse rutrum euismod eros, id posuere dolor rutrum vel. Mauris sed hendrerit libero. Vivamus dignissim scelerisque felis, ac fermentum mauris suscipit ut. Vivamus sed mattis magna. Donec ultrices ex quis libero vestibulum, vitae porta erat dictum. Integer ut nulla lacus. Fusce tincidunt urna eget accumsan aliquet.'),
(12, 1, 'Baran', 'Igor', 'Kłuszyńska 19 71-809 Szczecin', 'Profesor', 'https://ibaran.website.void', 'Curabitur efficitur, nulla tincidunt imperdiet pharetra, nisi massa sodales nisi, et ultrices nibh felis a diam. Donec varius purus nec enim gravida, sit amet interdum enim faucibus. Duis sit amet volutpat purus, quis dignissim diam. Fusce et quam lectus. Aliquam luctus ante vel gravida pulvinar. Sed nec nisl eu eros auctor dapibus. Etiam vitae dolor purus. Maecenas ac mattis nunc. Integer dapibus consectetur interdum. Cras blandit purus gravida erat lacinia gravida. Curabitur luctus, nisl sit amet elementum lobortis, ante ipsum placerat ligula, sit amet consectetur ante justo sed eros.'),
(13, 2, 'Duda', 'Paulina', 'Poniatowskiego Józefa 36 15-199 Białystok', 'Doktor', 'https://pduda.website.void', 'Etiam sagittis orci at metus dapibus, nec commodo metus bibendum. Aliquam in purus accumsan, iaculis felis vitae, dignissim libero. Nunc aliquet tempus eleifend. In ut augue nunc. Nullam congue risus quis nisi eleifend, non accumsan nulla convallis. Praesent enim elit, lacinia nec diam vitae, tincidunt auctor justo. Vestibulum mauris ipsum, ornare a auctor ac, suscipit et neque. Aliquam egestas ex sed dictum interdum. Ut efficitur tincidunt arcu, sed pellentesque tellus ultrices sit amet. Ut tristique eu purus eu posuere. Mauris non elit non neque pretium fringilla. Aliquam sollicitudin, quam ut porttitor sagittis, metus libero porttitor nisl, sit amet imperdiet lectus nibh eget ligula.'),
(14, 3, 'Kaczmarczyk', 'Agata', 'Ropczycka 64 61-316 Poznań', 'Magister', 'https://akaczmarczyk.website.void', 'Suspendisse vel lacinia nisl. Pellentesque gravida gravida mauris. Vivamus egestas volutpat est id fermentum. Nulla sed enim viverra, ultricies ipsum quis, bibendum dolor. Aenean eu massa egestas erat sodales iaculis. Suspendisse non est sit amet dui sodales venenatis vitae id ipsum. Sed congue leo vel tincidunt auctor. Proin dictum dignissim libero at pharetra. Morbi suscipit, elit in convallis maximus, velit mi porttitor justo, eu placerat risus sem non nisl. In aliquam scelerisque commodo. Pellentesque maximus, enim sed varius placerat, mauris mi iaculis velit, sed consectetur lectus neque eu purus. Nulla ut erat massa.');

-- SL_CourseStates --
INSERT INTO SL_CourseStates VALUES
(1, 'Created'),
(2, 'Opened'),
(3, 'Started'),
(4, 'Finished');

-- Kurs --
INSERT INTO Courses VALUES
(1, 1, 'Proin sit amet sem vitae mi venenatis sagittis eu vel est.', 4, 2, 'Curabitur varius ante imperdiet nulla varius lacinia. Pellentesque mauris erat, varius non sodales id, pretium vel nisi. Mauris aliquet ut dolor varius molestie. Praesent pharetra lobortis dui. Suspendisse aliquam vestibulum vehicula. Fusce ante dolor, fringilla eu nulla sed, ullamcorper iaculis sem. Morbi pulvinar massa mauris, vel accumsan risus rutrum sit amet. Sed venenatis volutpat tortor, vitae porta nisi dictum id. Donec arcu velit, faucibus et leo sed, euismod placerat lacus. Donec consequat pretium lorem ac accumsan. Suspendisse vel lobortis mauris. Curabitur iaculis vulputate metus, eu condimentum mauris tincidunt vel.', '20161001', '20161101'),
(1, 1, 'Curabitur dignissim dui eu risus dapibus ultricies vitae id justo.', 4, 2, 'Etiam aliquet tempus ullamcorper. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris blandit tincidunt purus eu luctus. Maecenas at sem sit amet dui molestie mollis. Morbi nec diam nec magna interdum dictum. Nulla sit amet odio turpis. Vivamus porttitor et ipsum hendrerit rhoncus. Sed a nibh vulputate mauris bibendum sagittis.', '20161002', '20161102'),
(2, 1, 'Ut pretium erat vel condimentum congue.', 4, 1, 'Nulla ac mollis lectus, id laoreet massa. Sed mollis tristique odio ac pellentesque. Cras hendrerit vulputate arcu, sed accumsan massa pulvinar eu. Vestibulum vitae faucibus ipsum. Integer congue tristique sem, ac condimentum dui maximus a. Vestibulum lectus est, hendrerit quis nunc sit amet, fermentum condimentum neque. Fusce nibh elit, imperdiet lacinia ultricies vel, posuere sit amet augue. Phasellus nisl magna, mollis eu ultrices non, molestie ac sem.', '20161003', '20161103'),
(2, 2, 'Ut ac velit in dui faucibus semper.', 4, 1, 'Integer eu dapibus magna. Vivamus egestas arcu sem, consequat sodales elit dignissim id. Donec diam urna, ultrices eget tortor a, condimentum iaculis quam. Nam sollicitudin ultricies mauris sit amet commodo. Morbi cursus a risus at finibus. Nunc laoreet leo mollis lorem aliquam, sit amet venenatis risus malesuada. Nunc tellus mauris, sodales non leo quis, fermentum hendrerit urna. Maecenas feugiat finibus laoreet. Integer dolor ipsum, tempor at ligula non, commodo sodales dolor. Fusce vulputate tempus justo. In hac habitasse platea dictumst. Ut mi neque, viverra vitae vehicula quis, accumsan nec sem. Aliquam erat volutpat. Praesent eu erat id lorem tempor hendrerit.', '20161004', '20161104'),
(4, 2, 'Pellentesque iaculis metus ac condimentum rutrum.', 4, 1, 'Mauris massa elit, luctus quis dapibus eu, viverra nec nisl. Mauris efficitur felis sed est bibendum tempus. Proin neque ex, sagittis nec iaculis ac, scelerisque in urna. Aenean faucibus lacus id tellus lacinia consectetur eu non ante. Nulla augue nisl, sollicitudin sed arcu sed, finibus vulputate orci. Vestibulum sed eleifend quam. Integer pulvinar sed enim sed semper. Cras porttitor quis sapien sed placerat. Nam ante libero, pharetra at dolor ultricies, vehicula ullamcorper purus. Integer venenatis libero ante, ut vulputate massa malesuada sed.', '20161005', '20161105'),
(5, 2, 'Sed lobortis nisi in turpis vulputate aliquet sit amet quis ligula.', 4, 1, 'Nulla sed enim sem. Ut placerat aliquam mi, at lacinia felis. In imperdiet tortor euismod dolor elementum hendrerit. Etiam justo elit, condimentum nec dictum et, imperdiet quis erat. Cras molestie eget lectus sit amet rhoncus. Curabitur vulputate ac nunc vitae convallis. Aliquam ultrices egestas nulla, blandit hendrerit nulla. In mollis eget lacus sit amet ullamcorper. Fusce auctor magna eu sodales feugiat. Vestibulum eget libero orci. Mauris feugiat mauris dolor, id maximus ex mattis sit amet.', '20161006', '20161106');

-- Grupa --
INSERT INTO Groups VALUES --4 studentów, 6 kursów
-- (1, 1),
-- (1, 2),
-- (1, 3),
-- (1, 4),
-- (1, 5),
-- (1, 6),
(2, 1),
(2, 2),
(2, 3),
(2, 4),
(2, 5),
(2, 6),
(4, 1),
(4, 2),
(4, 3),
(4, 4),
(4, 5),
(4, 6),
(5, 1),
(5, 2),
(5, 3),
(5, 4),
(5, 5),
(5, 6);

-- Zajecia --
INSERT INTO Lectures VALUES
(1, 1, '20161011'),
(1, 1, '20161007'),
(1, 1, '20161008'),
(1, 1, '20161009'),
(2, 1, '20161023'),
(2, 1, '20161010'),
(2, 2, '20161014'),
(2, 2, '20161018'),
(3, 2, '20161016'),
(3, 2, '20161006'),
(3, 2, '20161024'),
(3, 2, '20161025'),
(4, 4, '20161026'),
(4, 4, '20161028'),
(4, 4, '20161022'),
(4, 4, '20161012'),
(5, 4, '20161013'),
(5, 4, '20161019'),
(5, 5, '20161017'),
(5, 5, '20161015'),
(6, 5, '20161027'),
(6, 5, '20161029'),
(6, 5, '20161020'),
(6, 5, '20161021');

-- Obecnosci --
INSERT INTO Attendance VALUES
(1,  1, 1),
(1,  2, 1),
(1,  4, 1),
(1,  5, 1),
(2,  1, 1),
(2,  2, 1),
(2,  4, 1),
(2,  5, 1),
(3,  1, 1),
(3,  2, 1),
(3,  4, 1),
(3,  5, 1),
(4,  1, 1),
(4,  2, 1),
(4,  4, 1),
(4,  5, 1),
(5,  1, 1),
(5,  2, 1),
(5,  4, 1),
(5,  5, 1),
(6,  1, 1),
(6,  2, 1),
(6,  4, 1),
(6,  5, 1),
(7,  1, 1),
(7,  2, 1),
(7,  4, 1),
(7,  5, 1),
(8,  1, 1),
(8,  2, 1),
(8,  4, 1),
(8,  5, 1),
(9,  1, 1),
(9,  2, 1),
(9,  4, 1),
(9,  5, 1),
(10, 1, 1),
(10, 2, 1),
(10, 4, 1),
(10, 5, 1),
(11, 1, 1),
(11, 2, 1),
(11, 4, 1),
(11, 5, 1),
(12, 1, 1),
(12, 2, 1),
(12, 4, 1),
(12, 5, 1),
(13, 1, 1),
(13, 2, 1),
(13, 4, 1),
(13, 5, 1),
(14, 1, 1),
(14, 2, 1),
(14, 4, 1),
(14, 5, 1),
(15, 1, 1),
(15, 2, 1),
(15, 4, 1),
(15, 5, 1),
(16, 1, 1),
(16, 2, 1),
(16, 4, 1),
(16, 5, 1),
(17, 1, 1),
(17, 2, 1),
(17, 4, 1),
(17, 5, 1),
(18, 1, 1),
(18, 2, 1),
(18, 4, 1),
(18, 5, 1),
(19, 1, 1),
(19, 2, 1),
(19, 4, 1),
(19, 5, 1),
(20, 1, 1),
(20, 2, 1),
(20, 4, 1),
(20, 5, 1),
(21, 1, 1),
(21, 2, 1),
(21, 4, 1),
(21, 5, 1),
(22, 1, 1),
(22, 2, 1),
(22, 4, 1),
(22, 5, 1),
(23, 1, 1),
(23, 2, 1),
(23, 4, 1),
(23, 5, 1),
(24, 1, 1),
(24, 2, 1),
(24, 4, 1),
(24, 5, 1);

-- Oceny --
INSERT INTO Grades VALUES
(1, 1, 5.0, '20161101', 1, 'Etiam sit amet leo id dolor faucibus pellentesque.'),
(2, 1, 4.5, '20161101', 1, 'Suspendisse varius nisi non massa rutrum, nec mattis elit convallis.'),
(4, 1, 4.0, '20161101', 1, 'Mauris sit amet eros sit amet nibh auctor semper.'),
(5, 1, 3.5, '20161101', 1, 'Sed non ante in velit mollis rhoncus.'),
(1, 2, 3.0, '20161102', 1, 'Nullam congue turpis luctus sodales egestas.'),
(2, 2, 5.0, '20161102', 1, 'Nam auctor enim sollicitudin massa interdum, non ultricies leo feugiat.'),
(4, 2, 4.5, '20161102', 1, 'Donec et eros imperdiet arcu pulvinar viverra non ac nisl.'),
(5, 2, 4.0, '20161102', 1, 'Etiam sed ante a lacus venenatis laoreet at eu diam.'),
(1, 3, 3.5, '20161103', 2, 'Donec a velit placerat, mattis lacus dignissim, feugiat mauris.'),
(2, 3, 3.0, '20161103', 2, 'Donec tincidunt purus ac ligula sollicitudin, vitae porta dui eleifend.'),
(4, 3, 5.0, '20161103', 2, 'Fusce in urna aliquam lectus sodales blandit quis gravida lorem.'),
(5, 3, 4.5, '20161103', 2, 'Suspendisse non mi in metus vestibulum iaculis.'),
(1, 4, 4.0, '20161104', 2, 'Nulla eget lacus posuere neque ultrices tristique.'),
(2, 4, 3.5, '20161104', 2, 'Sed ut lorem ullamcorper, condimentum risus a, ullamcorper felis.'),
(4, 4, 3.0, '20161104', 2, 'Donec venenatis nibh id sapien sagittis vehicula.'),
(5, 4, 5.0, '20161104', 2, 'Fusce sed arcu non urna cursus posuere quis nec urna.'),
(1, 5, 4.5, '20161105', 4, 'Nunc ac nisl ac turpis volutpat vestibulum sit amet imperdiet turpis.'),
(2, 5, 4.0, '20161105', 4, 'Sed viverra justo consequat euismod pulvinar.'),
(4, 5, 3.5, '20161105', 4, 'Cras blandit erat sed lectus bibendum rhoncus.'),
(5, 5, 3.0, '20161105', 4, 'Fusce id magna in nunc ultricies lobortis.'),
(1, 6, 5.0, '20161106', 5, 'Nullam et nunc iaculis massa vestibulum efficitur quis ultrices ante.'),
(2, 6, 4.5, '20161106', 5, 'Duis blandit urna at diam convallis dignissim.'),
(4, 6, 4.0, '20161106', 5, 'Proin eget lorem a est dictum pulvinar in in erat.'),
(5, 6, 3.5, '20161106', 5, 'Sed convallis quam eget finibus laoreet.');

-- SL_StanKomentarza --
INSERT INTO SL_CommentState VALUES
(1, 'Negative'),
(2, 'Neutral'),
(3, 'Positive'),
(4, 'Locked');

-- Komentarze --
INSERT INTO Comments VALUES
(1, 1, '20171111', 'In sapien leo, scelerisque et aliquam et, ultrices non tellus. Maecenas mi ligula, facilisis vitae dolor non, cursus cursus mi. Aliquam faucibus ex quis enim semper placerat. Quisque ornare et massa eget faucibus. Aenean suscipit lectus sem, cursus placerat orci commodo et. Fusce efficitur risus volutpat nisi commodo ornare. Aenean non tempor augue. Duis sed tortor est. Fusce sed urna a risus venenatis tristique. Sed dignissim pretium viverra. Ut at dui elementum, fermentum turpis nec, faucibus eros.', 1),
(2, 1, '20171111', 'Duis ac neque a turpis elementum imperdiet. In hac habitasse platea dictumst. Proin tincidunt condimentum tincidunt. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Proin et aliquam metus. Donec egestas efficitur semper. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Quisque scelerisque sapien orci, vel faucibus lorem fermentum et. Quisque a blandit elit, eu maximus arcu.', 2),
(4, 1, '20171111', 'In accumsan cursus arcu, eget luctus libero luctus sagittis. Curabitur ac pellentesque dolor, in laoreet arcu. Fusce non augue dapibus, imperdiet lectus ac, bibendum lectus. Mauris nec arcu ac dui cursus tincidunt vel vitae sem. Curabitur arcu libero, scelerisque a turpis in, accumsan semper ligula. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean condimentum eros ultrices erat cursus eleifend.', 3),
(5, 1, '20171111', 'Vivamus auctor dictum nisl non eleifend. Aenean vitae malesuada quam. Vestibulum eros turpis, semper venenatis feugiat non, euismod ac ipsum. Integer porta dignissim ligula vel euismod. Donec in semper dui. Nunc et erat at metus ornare pellentesque vel a nisi. Nam quis vestibulum lacus. Nulla facilisi. Suspendisse nisi leo, ornare quis laoreet eu, feugiat et orci. Nam eget eros ligula. Nunc mattis hendrerit leo eget sagittis. Quisque quis neque vehicula, dignissim lorem at, facilisis ligula. Etiam vestibulum nulla ac lacinia consequat. Aenean vel iaculis ex, ut convallis elit. In consequat massa quis placerat tempor.', 3),
(1, 2, '20171112', 'Aliquam mauris erat, imperdiet ut placerat in, volutpat rhoncus velit. Suspendisse ac feugiat risus, eget maximus quam. Curabitur ultrices porttitor mauris non consectetur. Pellentesque semper quis mi mollis fermentum. Aenean tincidunt lacus nec lectus pretium, non cursus enim faucibus. Phasellus efficitur aliquet ornare. Donec et odio eget lorem sollicitudin fermentum vel at enim. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. In hac habitasse platea dictumst. Praesent urna augue, mattis quis auctor a, fermentum sed justo. Vivamus pharetra ut lacus eget efficitur.', 2),
(2, 2, '20171112', 'Praesent vitae rutrum velit. Cras magna lacus, ornare at mauris at, rhoncus imperdiet mi. Sed mauris nibh, iaculis at rhoncus sed, volutpat id massa. Nulla aliquam mi sit amet laoreet dignissim. Donec congue lorem eu quam maximus ultrices. Vestibulum a tellus in augue tempor efficitur. Pellentesque turpis ipsum, pharetra nec enim in, maximus interdum lacus.', 3),
(4, 2, '20171112', 'Vestibulum at hendrerit elit, pharetra mollis velit. Duis vel ligula ultrices, euismod sapien tincidunt, rutrum ante. Vivamus cursus tempor quam, sit amet vestibulum eros porta a. Donec luctus maximus tortor at rhoncus. Mauris porta auctor nibh sed hendrerit. Nam nisi felis, vestibulum ut maximus nec, elementum ut sem. Nulla blandit ac diam eu hendrerit. Aenean sit amet pulvinar ex, sed semper diam. Pellentesque vitae imperdiet magna.', 2),
(5, 2, '20171112', 'Ut erat metus, tristique a bibendum in, venenatis in tellus. Nam non erat fringilla ante condimentum porttitor. Etiam sit amet neque dui. Nunc sit amet quam a metus cursus placerat. Nulla hendrerit nulla ac elementum pharetra. Phasellus pellentesque diam eget tempus dignissim. Vivamus volutpat sem a consequat vulputate.', 1),
(1, 3, '20171113', 'Sed a erat felis. Mauris vel ligula nulla. Morbi vehicula ligula nec tincidunt imperdiet. Duis egestas rutrum ex, in scelerisque orci vulputate vel. Quisque vel quam eu ex venenatis tristique quis eget massa. Donec a augue eget justo imperdiet ullamcorper. Suspendisse id vulputate odio.', 2),
(2, 3, '20171113', 'In tincidunt urna tellus. Etiam at ornare dui. Pellentesque aliquam bibendum nibh vitae blandit. Integer euismod et lectus non egestas. Vivamus iaculis bibendum fringilla. Nulla non orci lacus. Phasellus laoreet nisl ante, vel placerat purus vestibulum at. Donec ornare velit a finibus posuere.', 4),
(4, 3, '20171113', 'Etiam varius lobortis consequat. Sed porta elit ac consequat ultrices. Cras ac dapibus nibh. Fusce dignissim pharetra accumsan. Integer luctus molestie bibendum. Fusce quis dictum mauris. Aenean nec cursus ligula. Integer ex leo, imperdiet eget ante quis, euismod faucibus tortor. Nullam eros orci, molestie vel erat sollicitudin, malesuada ultricies lacus.', 2),
(5, 3, '20171113', 'Ut sed ante maximus, ullamcorper magna id, rutrum odio. Mauris aliquet mollis turpis, eget porta tortor consequat sit amet. Sed sagittis lectus erat, et tincidunt ligula vestibulum eu. Nam diam sapien, dapibus non sapien vel, malesuada laoreet massa. Vestibulum mollis magna sed urna porta, nec tincidunt lacus elementum. Aliquam congue justo a mauris ornare facilisis. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos.', 4),
(1, 4, '20171114', 'Vivamus elementum quam vel massa viverra, vitae sagittis sapien feugiat. Nulla maximus, elit at maximus lobortis, magna magna posuere nulla, sit amet interdum mi ante sed velit. Vestibulum ac varius ipsum. Aliquam et consequat velit. Praesent eu molestie lectus. Donec vehicula sollicitudin lacus eu tincidunt. Nunc ornare cursus mi, vel hendrerit risus. Phasellus volutpat dui ac nisi consectetur accumsan.', 2),
(2, 4, '20171114', 'Quisque scelerisque pretium vestibulum. Curabitur sit amet faucibus diam. Vestibulum dui turpis, faucibus sit amet arcu quis, bibendum auctor felis. Fusce suscipit volutpat mauris, auctor maximus lacus pretium vel. Vestibulum in aliquet ligula, sit amet fermentum tortor. Nulla facilisis blandit fringilla. Mauris ac libero justo.', 3),
(4, 4, '20171114', 'Nulla non est sed ligula luctus hendrerit. Ut nulla dui, maximus vitae lorem et, congue aliquet quam. Etiam vulputate nisl eget libero tincidunt suscipit. Nunc tristique condimentum nibh eget ullamcorper. Aenean auctor turpis vel est dapibus, at tincidunt mi ultrices. Ut vitae nunc augue. Sed et tristique est. Ut ac sodales urna. Donec condimentum orci interdum ipsum mattis, semper placerat lacus mollis.', 3),
(5, 4, '20171114', 'Etiam tempus non justo vitae commodo. Duis id hendrerit nulla, at fringilla velit. Ut venenatis eros euismod nisl efficitur sollicitudin. Aliquam lectus augue, viverra imperdiet lectus eu, hendrerit ullamcorper tellus. Morbi id sollicitudin ante, nec efficitur lectus. Cras condimentum, sem a facilisis egestas, enim dolor eleifend urna, nec gravida elit metus eget nisi.', 2),
(1, 5, '20171115', 'Phasellus mattis, dolor vel consequat auctor, enim nisl placerat ante, ac tristique ipsum metus sed purus. Praesent sit amet massa vel nulla posuere pellentesque ac ac odio. Fusce at tempor nisi. Aliquam vitae commodo urna. Aliquam malesuada leo in massa tristique varius at quis lorem.', 1),
(2, 5, '20171115', 'Sed mollis neque finibus, lobortis neque at, aliquam mauris. Mauris ultrices justo quis nibh egestas ultricies. Etiam congue nulla nunc, sed porttitor massa sagittis vel. Aenean lobortis placerat odio vel convallis. Cras luctus nunc at arcu semper finibus. Fusce vulputate placerat tellus a ullamcorper. In scelerisque ut orci vel pellentesque. Suspendisse mollis maximus condimentum. Vestibulum bibendum quam ut ex cursus sollicitudin.', 1),
(4, 5, '20171115', 'Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nunc eu auctor felis, vitae rutrum libero. Nam finibus feugiat quam non posuere. Sed dictum massa tellus, eu eleifend ante malesuada eu. Aenean et pulvinar felis, quis dignissim lectus. Proin mollis finibus tempor. Suspendisse euismod quam ac pretium lobortis. Vivamus sed accumsan sem.', 2),
(5, 5, '20171115', 'Etiam mattis maximus turpis, non rhoncus leo mollis ut. Curabitur vitae purus imperdiet, elementum arcu a, interdum purus. Cras vitae neque sagittis, accumsan enim eget, suscipit elit. Integer eu vestibulum nulla. Phasellus ullamcorper nisi non sollicitudin pulvinar. Donec ut massa quam. Morbi ac mi mollis, fermentum orci ac, accumsan ipsum.', 3),
(1, 6, '20171116', 'Donec pretium felis nibh, nec consectetur nunc blandit non. Phasellus luctus enim at ullamcorper facilisis. Donec vitae neque id urna molestie lacinia. Suspendisse viverra nisi a nunc elementum, vitae porttitor sapien porta. Cras eu imperdiet nunc. Suspendisse tincidunt, metus quis semper mollis, nunc lorem commodo mauris, eu accumsan erat turpis vitae tortor. Mauris ut molestie augue. Pellentesque sit amet viverra elit.', 2),
(2, 6, '20171116', 'Curabitur nisi metus, bibendum eu elementum eu, mattis vel diam. Proin elementum in metus eu vestibulum. Cras bibendum tellus sem, id lobortis sem molestie sit amet. Suspendisse mollis id nisi eget finibus. Nullam tempus nisl at nisi mollis varius. Vestibulum placerat purus sed mi condimentum dapibus. Donec eu porttitor risus.', 1),
(4, 6, '20171116', 'Mauris finibus est sit amet magna porttitor, ac sagittis purus luctus. Aliquam consequat facilisis orci, ut interdum orci eleifend quis. Etiam elementum magna a mi cursus auctor. Phasellus quis lectus finibus, egestas arcu ac, suscipit neque. Donec aliquet nibh risus, et lobortis ligula viverra sed. Donec mattis felis ac nunc ornare, vitae hendrerit odio mattis. Sed convallis leo turpis, id pellentesque justo ultricies vitae.', 3),
(5, 6, '20171116', 'In ut elit odio. Nunc nec risus vitae elit convallis condimentum. Etiam justo sem, molestie eget posuere sed, porta at diam. Maecenas tempus, magna at fermentum congue, mauris orci auctor nisi, at tempor leo nibh nec lectus. Aliquam consequat consequat facilisis. Morbi porta id turpis ac iaculis. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Maecenas elementum aliquam gravida.', 2);

-- EF_UserRoles --
INSERT INTO EF_UserRoles VALUES
(1,  1),
(2,  1),
(3,  2),
(4,  2),
(5,  2),
(6,  3),
(7,  3),
(8,  3),
(9,  4),
(10, 4),
(11, 4),
(12, 2),
(12, 4),
(13, 2),
(13, 4),
(14, 2),
(14, 4);
