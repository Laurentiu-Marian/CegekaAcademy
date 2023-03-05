1. La delete am pus return-uri "null" pentru ca nu stiam ce sa intorc pentru Task ActionResult
2. Cand creez un fundraiser nou, indiferent daca persona exista, el mai creeaza una
	Nu stiu de ce nu vrea sa functioneze cum trebuie verificarea dupa IdNumber
3. La Persons am adaugat o coloana FundraiserCreatorId (care e NULL) gandindu-ma ca imi trebuie la 
	crearea de Fundraisers, deocamdata nu stiu ce sa fac cu ea pentru ca nu pot obtine Id-ul noului
	Fundraiser pentru ca la Domain nu e si nu stiu/nu cred ca e ok sa il adaug.
	Poate cu inca cateva metode de search s-ar putea rezolva, dar nu imi dau seama cum, si la fel
	si problema de la 2. posibil sa dispara
4. Eu am facut cat de cat cele 5 metode la controler, nu mi-am dat seama daca in cerinta mai pe final
	se cerea si posibilitatea de a dona din Api sau era doar pentru a intelege cum ar trebui sa
	arate clasele.
5. Sunt doua migrari